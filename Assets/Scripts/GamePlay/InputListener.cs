using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public event Action Jump;
    public event Action GravityChange; 
    public event Action Attack;
    public event Action<Vector2Int> Navigate;
    public event Action Pause;
    public event Action Accept;
    public event Action CancelJump;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z)||Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Jump != null) Jump();
            Accept?.Invoke();
        }
        
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Z)||Input.GetKeyUp(KeyCode.UpArrow))
        {
            CancelJump?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space)  || Input.GetKeyDown(KeyCode.C))
        {
            if (GravityChange != null) GravityChange();
        }

        // if (Input.GetKeyDown(KeyCode.F)  || Input.GetKeyDown(KeyCode.X))
        // {
        //     if (Attack != null) Attack();
        // }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Pause?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.A))
        {
            Navigate?.Invoke(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.D))
        {
            Navigate?.Invoke(Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W))
        {
            Navigate?.Invoke(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.S))
        {
            Navigate?.Invoke(Vector2Int.down);
        }
    }
}
