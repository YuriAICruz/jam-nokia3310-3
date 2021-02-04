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

    private void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            if (Jump != null) Jump();
            Accept?.Invoke();
        }

        if (Input.GetKeyDown("space"))
        {
            if (GravityChange != null) GravityChange();
            Accept?.Invoke();
        }

        if (Input.GetKeyDown("f"))
        {
            if (Attack != null) Attack();
            Accept?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Navigate?.Invoke(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Navigate?.Invoke(Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Navigate?.Invoke(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Navigate?.Invoke(Vector2Int.down);
        }
    }
}
