using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public static event Action Jump;
    public static event Action GravityChange; 
    public static event Action Attack;
    public static event Action<Vector2Int> Navigate;

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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            
        }
    }
}
