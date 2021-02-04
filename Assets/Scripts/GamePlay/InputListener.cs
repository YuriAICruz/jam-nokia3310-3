using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public delegate void JumpAct(); 
    public  event JumpAct Jump;
    public delegate void GravChange(); 
    public  event GravChange GravityChange;
    public delegate void AttAct(); 
    public  event AttAct Attack;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            if (Jump != null) Jump();
        }

        if (Input.GetKeyDown("space"))
        {
            if (GravityChange != null) GravityChange();
        }

        if (Input.GetKeyDown("f"))
        {
            if (Attack != null) Attack();
        }
    }
}
