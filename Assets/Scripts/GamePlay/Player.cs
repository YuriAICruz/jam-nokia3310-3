using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Physics = UnityEngine.Physics;

public class Player : MonoBehaviour
{
    public static Player Instace;
    public Physics ps= new Physics();
    void Start()
    {
        Instace = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameStop();
    }

    private void GameStop()
    {
        Bootstrap.Instance.GameSystem.state = GameSystem.GameStates.GameOver;
        Bootstrap.Instance.ScoreManager.CheckHigScore();
        
    }

    private void FixedUpdate()
    {
        
    }
}
