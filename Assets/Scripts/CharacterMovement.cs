using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public int moveCount = 0;//记录move的帧数

    public int moveMode = 0;//决定人物平移还是旋转：1为平移，2为旋转

    public int activateMove = 0;//是否启动移动事件

    public float h;//旋转量

    public float v;//位移量

    public float moveCharacter(float key)//控制人物移动的方法
    {
        if (moveCount > 0 && moveMode != 3)
        {
            key = 1;
            moveCount++;
            //Debug.Log(moveCount);
        }

        if (moveCount < 0 && moveMode == 3)
        {
            key = -1;
            moveCount--;
            //Debug.Log(moveCount);
        }
        if (moveMode == 1 && moveCount == 38)//平移模式
        {
            moveCount = 0;
            moveMode = 0;
            key = 0;
        }
        if (moveMode == 2 && moveCount == 47)//右旋转模式
        {
            moveCount = 0;
            moveMode = 0;
            key = 0;
        }
        if (moveMode == 3 && moveCount == -47)//左旋转模式
        {
            moveCount = 0;
            moveMode = 0;
            key = 0;
        }
        return key;
    }

    public void transCharacter()
    {
        moveCount = 1;
        moveMode = 1;
    }

    public void rotateCharacter_Right()
    {
        moveCount = 1;
        moveMode = 2;
    }

    public void rotateCharacter_Left()
    {
        moveCount = -1;
        moveMode = 3;
    }
}
