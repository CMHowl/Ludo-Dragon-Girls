//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// 必要なコンポーネントの列記
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]

public class UnityChanControlScriptWithRgidBody : MonoBehaviour
{

	public float animSpeed = 1.5f;				// アニメーション再生速度設定
	public float lookSmoother = 3.0f;			// a smoothing setting for camera motion
	public bool useCurves = true;				// Mecanimでカーブ調整を使うか設定する
												// このスイッチが入っていないとカーブは使われない
	public float useCurvesHeight = 0.5f;		// カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

	// 以下キャラクターコントローラ用パラメタ
	// 前進速度
	public float forwardSpeed = 7.0f;
	// 後退速度
	public float backwardSpeed = 2.0f;
	// 旋回速度
	public float rotateSpeed = 2.0f;
	// ジャンプ威力
	public float jumpPower = 3.0f; 
	// キャラクターコントローラ（カプセルコライダ）の参照
	private CapsuleCollider col;
	private Rigidbody rb;
	// キャラクターコントローラ（カプセルコライダ）の移動量
	private Vector3 velocity;
	// CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
	private float orgColHight;
	private Vector3 orgVectColCenter;
	
	private Animator anim;							// キャラにアタッチされるアニメーターへの参照
	private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照

	private GameObject cameraObject;    // メインカメラへの参照

    public int moveCount = 0;//记录move的帧数

    public int moveMode = 0;//决定人物平移还是旋转：1为平移，2为右旋转，3为左旋转

    public int activateMove = 0;//是否启动移动事件

    public float h;//旋转量

    public float v;//位移量

    public bool startLine = true;//判断是否刚刚从基地出发

    private bool rotateLock = false;//旋转锁，用于旋转操作间的互斥

    private bool transLock = false;//移动锁，用于旋转与位移间的互斥

    private Queue<MyStatus> statusQueue;//队列用于存放未执行的移动指令

    public int transLoop = 0;//记录移动的步数

    private GameObject otherCollider;

    public int maintainStand = 0;//检测物体是否处于静止状态

// アニメーター各ステートへの参照
	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int restState = Animator.StringToHash("Base Layer.Rest");

    class MyStatus
    {
        public int moveCount;//记录当前状态下已执行计数器
        public int extraLoop;//记录除当前状态之外未执行的总移动指令数
        public float speed;//记录当前加速倍率，防止两个2倍速叠加时后者被清空速度

        public MyStatus(int moveCount, int extraLoop,float speed)
        {
            this.moveCount = moveCount;
            this.extraLoop = extraLoop;
            this.speed = speed;
        }
    };


    // 初期化
    void Start ()
	{
		// Animatorコンポーネントを取得する
		anim = GetComponent<Animator>();
		// CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
		col = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();
		//メインカメラを取得する
		cameraObject = GameObject.FindWithTag("MainCamera");
		// CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
		orgColHight = col.height;
		orgVectColCenter = col.center;
        statusQueue = new Queue<MyStatus>();
}


float moveCharacter(float key)//控制人物移动的方法
    {
        if (moveCount > 0&&moveMode!=3)
        {
            key = 1;
            moveCount++;
            //Debug.Log(moveCount);
        }

        if(moveCount < 0 && moveMode == 3)
        {
            key = -1;
            moveCount--;
            //Debug.Log(moveCount);
        }
        if (moveMode==1 &&moveCount == 38)//平移模式
        {
            if (transLoop == 0)//单步移动，则还原操作
            {
                moveCount = 0;
                moveMode = 0;
                key = 0;
            }
            else//计数器置1，再循环一次
            {
                transLoop--;
                transCharacter();
            }
        }
        if(moveMode == 2 && moveCount == 47)//右旋转模式
        {
            moveCount = 0;
            moveMode = 0;
            key = 0;
            try
            {
                transform.position = Vector3.Lerp(transform.position, otherCollider.transform.position, 0.001f * Time.deltaTime);
            }
            catch (System.NullReferenceException)
            {
                throw;//检测到异常(没有碰撞体)，顺手扔了就完事了
            }
            
            //这里可能抛出异常，非碰撞产生的转弯动作是没有碰撞体的
            transLock = false;//释放移动锁
        }
        if (moveMode == 3 && moveCount == -47)//左旋转模式
        {
            moveCount = 0;
            moveMode = 0;
            key = 0;
            try
            {
                transform.position = Vector3.Lerp(transform.position, otherCollider.transform.position, 0.001f * Time.deltaTime);
            }
            catch (System.NullReferenceException)
            {
                throw;//检测到异常(没有碰撞体)，顺手扔了就完事了
            }
            transLock = false;//释放移动锁
        }
        maintainStand = 0;//只要当前有动作，就认为该状态为非静止
        return key;
    }

    public void transCharacterLoop(int diceCount)//按骰子点数移动调用
    {
        transLoop = diceCount-1;
        transCharacter();
    }

    public void QuickTransCharacterLoop(int diceCount)//按点数快速移动
    {
        forwardSpeed = 14.0f;//2倍速移动
        transCharacterLoop(diceCount / 2);
        Invoke("useDefaultforwardSpeed", diceCount * Time.fixedDeltaTime * 37);//创建委托
    }

    private void useDefaultforwardSpeed()
    {
        forwardSpeed = 7.0f;
    }

    public void transCharacter()//单步移动调用接口
    {
        if (transLock == false)//未上锁时，直接执行
        {
            moveCount = 1;
            moveMode = 1;
        }
        else //加锁时，保存当前状态，存入
        {
            statusQueue.Enqueue(new MyStatus(0, 1,forwardSpeed));
        }
    }

    public void rotateCharacter_Right()
    {
        if(moveCount>=1) statusQueue.Enqueue(new MyStatus(37, 0, forwardSpeed));
        moveCount = 1;
        moveMode = 2;
        transLock = true;//移动加锁
    }

    public void rotateCharacter_Left()
    {
        if (moveCount >= 1) statusQueue.Enqueue(new MyStatus(37, 0, forwardSpeed));//还原反而出现误差？
        moveCount = -1;
        moveMode = 3;
        transLock = true;//移动加锁
    }

    private void dequeueAction()
    {
        Debug.Log("Dequeue!"+ statusQueue.Count);
        MyStatus tmp = statusQueue.Dequeue();
        if (tmp.moveCount > 0)
        {
            moveCount = tmp.moveCount;
            forwardSpeed = tmp.speed;
            moveMode = 1;
        }
        else if (tmp.extraLoop == 1)
        {
            moveCount = 1;
            moveMode = 1;
            forwardSpeed = tmp.speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        otherCollider = other.gameObject;

        if (other.gameObject.name == "TCTrigger"&& startLine == true)//处理棋子刚刚走出基地的左转弯情况
        {
            rotateCharacter_Left();//抢占式左转
            startLine = false;//有bug！！！为什么自动右转了！
            return;
        }

        if (other.gameObject.name == "TCTrigger" && startLine == false)//处理棋子遇到实转角右转情况
        {
            rotateCharacter_Right();//抢占式右转
            return;
        }

        if (other.gameObject.name == "VCTrigger" )//处理棋子遇到虚转角情况
        {
            rotateCharacter_Left();//先左转
            transCharacter();//再直走一格
            return;
        }
    }


    // 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
    void FixedUpdate ()
	{
	    h = Input.GetAxis("Horizontal");				// 入力デバイスの水平軸をhで定義
		v = Input.GetAxis("Vertical");				// 入力デバイスの垂直軸をvで定義

        if (Input.GetKeyDown(KeyCode.K)) transCharacter();
        if (Input.GetKeyDown(KeyCode.L)) rotateCharacter_Right();
        if (Input.GetKeyDown(KeyCode.J)) rotateCharacter_Left();
        if (Input.GetKeyDown(KeyCode.I)) transCharacterLoop(4);
        if (moveMode == 1) v = moveCharacter(v);
        if (moveMode == 2) h = moveCharacter(h);
        if (moveMode == 3) h = moveCharacter(h);
        if (moveMode == 0 && statusQueue.Count > 0) dequeueAction();
        //h = moveCharacter(2,h);

        if (v == 0 && h == 0) maintainStand ++;
        if (maintainStand >= 5)
        {
            maintainStand = 5;
            forwardSpeed = 7.0f;//补丁，顺手还原速度
        }
        anim.SetFloat("Speed", v);							// Animator側で設定している"Speed"パラメタにvを渡す
		anim.SetFloat("Direction", h); 						// Animator側で設定している"Direction"パラメタにhを渡す
		anim.speed = animSpeed;								// Animatorのモーション再生速度に animSpeedを設定する
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// 参照用のステート変数にBase Layer (0)の現在のステートを設定する
		rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする

        

        // 以下、キャラクターの移動処理
        velocity = new Vector3(0, 0, v);		// 上下のキー入力からZ軸方向の移動量を取得
		// キャラクターのローカル空間での方向に変換
		velocity = transform.TransformDirection(velocity);
		//以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
		if (v > 0.1) {
			velocity *= forwardSpeed;		// 移動速度を掛ける
		} else if (v < -0.1) {
			velocity *= backwardSpeed;	// 移動速度を掛ける
		}
		
		if (Input.GetButtonDown("Jump")) {	// スペースキーを入力したら

			//アニメーションのステートがLocomotionの最中のみジャンプできる
			if (currentBaseState.nameHash == locoState){
				//ステート遷移中でなかったらジャンプできる
				if(!anim.IsInTransition(0))
				{
                        //rb.transform.Translate(Vector3.up*1f);
						//rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
						anim.SetBool("Jump", true);		// Animatorにジャンプに切り替えるフラグを送る
				}
			}
		}
		

		// 上下のキー入力でキャラクターを移動させる
		transform.localPosition += (velocity * Time.fixedDeltaTime*0.992122f);

		// 左右のキー入力でキャラクタをY軸で旋回させる
		transform.Rotate(0, h * rotateSpeed, 0);	
	

		// 以下、Animatorの各ステート中での処理
		// Locomotion中
		// 現在のベースレイヤーがlocoStateの時
		if (currentBaseState.nameHash == locoState){
			//カーブでコライダ調整をしている時は、念のためにリセットする
			if(useCurves){
				resetCollider();
			}
		}
		// JUMP中の処理
		// 現在のベースレイヤーがjumpStateの時
		else if(currentBaseState.nameHash == jumpState)
		{
			cameraObject.SendMessage("setCameraPositionJumpView");	// ジャンプ中のカメラに変更
			// ステートがトランジション中でない場合
			if(!anim.IsInTransition(0))
			{
				
				// 以下、カーブ調整をする場合の処理
				if(useCurves){
					// 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
					// JumpHeight:JUMP00でのジャンプの高さ（0〜1）
					// GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
					float jumpHeight = anim.GetFloat("JumpHeight");
					float gravityControl = anim.GetFloat("GravityControl"); 
					if(gravityControl > 0)
						rb.useGravity = false;	//ジャンプ中の重力の影響を切る
										
					// レイキャストをキャラクターのセンターから落とす
					Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
					RaycastHit hitInfo = new RaycastHit();
					// 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
					if (Physics.Raycast(ray, out hitInfo))
					{
						if (hitInfo.distance > useCurvesHeight)
						{
							col.height = orgColHight - jumpHeight;			// 調整されたコライダーの高さ
							float adjCenterY = orgVectColCenter.y + jumpHeight;
							col.center = new Vector3(0, adjCenterY, 0);	// 調整されたコライダーのセンター
						}
						else{
							// 閾値よりも低い時には初期値に戻す（念のため）					
							resetCollider();
						}
					}
				}
				// Jump bool値をリセットする（ループしないようにする）				
				anim.SetBool("Jump", false);
			}
		}
		// IDLE中の処理
		// 現在のベースレイヤーがidleStateの時
		else if (currentBaseState.nameHash == idleState)
		{
			//カーブでコライダ調整をしている時は、念のためにリセットする
			if(useCurves){
				resetCollider();
			}
			// スペースキーを入力したらRest状態になる
			if (Input.GetButtonDown("Jump")) {
				anim.SetBool("Rest", true);
			}
		}
		// REST中の処理
		// 現在のベースレイヤーがrestStateの時
		else if (currentBaseState.nameHash == restState)
		{
			//cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
			// ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
			if(!anim.IsInTransition(0))
			{
				anim.SetBool("Rest", false);
			}
		}
	}


	void OnGUI()
	{
        //GUI.Box(new Rect(Screen.width -260, 10 ,250 ,150), "Interaction");
        //GUI.Label(new Rect(Screen.width -245,30,250,30),"Up/Down Arrow : Go Forwald/Go Back");
        //GUI.Label(new Rect(Screen.width -245,50,250,30),"Left/Right Arrow : Turn Left/Turn Right");
        //GUI.Label(new Rect(Screen.width -245,70,250,30),"Hit Space key while Running : Jump");
        //GUI.Label(new Rect(Screen.width -245,90,250,30),"Hit Spase key while Stopping : Rest");
        //GUI.Label(new Rect(Screen.width -245,110,250,30),"Left Control : Front Camera");
        //GUI.Label(new Rect(Screen.width -245,130,250,30),"Alt : LookAt Camera");
	}


	// キャラクターのコライダーサイズのリセット関数
	void resetCollider()
	{
	// コンポーネントのHeight、Centerの初期値を戻す
		col.height = orgColHight;
		col.center = orgVectColCenter;
	}

    //void moveCharacter()
    //{
    //}
}
