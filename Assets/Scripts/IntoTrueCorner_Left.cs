using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoTrueCorner_Left : MonoBehaviour {
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

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotateCount < 0 && rotateCount != -46)
        {
            myCharacter.transform.Rotate(0, -1 * (2f), 0);//左转
            rotateCount--;
        }
    }
}
