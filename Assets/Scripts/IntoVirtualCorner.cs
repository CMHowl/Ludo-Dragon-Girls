using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoVirtualCorner : MonoBehaviour
{
    public GameObject myCharacter;
    public int rotateCount = 0;
    public int transCount = 0;
    public CharacterMovement characterMovement;
    private void OnTriggerEnter(Collider other)
    {

        myCharacter = other.gameObject;
        rotateCount = -1;
        transCount = 1;
    }

    public void AutoGo(GameObject gameObject)
    {
        
        
    }


    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotateCount < 0 && rotateCount != -46)
        {
            myCharacter.transform.Rotate(0, -1 * (2f), 0);
            rotateCount--;
        }
        if (rotateCount == -46)
        {
            if (transCount > 0 && transCount != 38)
            {
                myCharacter.transform.localPosition += (myCharacter.transform.TransformDirection(new Vector3(0, 0, 7f)) * Time.fixedDeltaTime * 0.992122f);//再直走
                transCount++;
            }
        }
    }
}
