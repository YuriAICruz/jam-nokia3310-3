using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Tilemaps;
using Physics = DefaultNamespace.Physics;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public Physics physics;
    private Vector3 _newPosition;
    public float translationSpeed;
    private Vector3 _lastPosition;
    private GameSystem _gameSystem;
    private Vector3 _startPosition;

    void Awake()
    {
        Instance = this;

        physics = new Physics();

        if (physics.Move(transform.position, Vector3Int.zero, out var newPosition))
        {
            _newPosition = _lastPosition = newPosition;
            transform.position = physics.SetPosition(_newPosition);
        }

        _startPosition = transform.position;

        _gameSystem = Bootstrap.Instance.GameSystem;
        _gameSystem.GameStatesChange += GameStatesChange;
    }

    private void GameStatesChange(GameSystem.GameStates state, GameSystem.GameStates oldState)
    {
        if (state == GameSystem.GameStates.GameStart)
        {
            transform.position = _lastPosition = _newPosition = _startPosition;
        }
    }

    void Update()
    {
        var pos = _lastPosition = Vector3.Lerp(_lastPosition, _newPosition, Time.deltaTime * translationSpeed);
        transform.position = physics.SetPosition(pos);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameStop();
    }

    private void GameStop()
    {
        Bootstrap.Instance.GameSystem.State = GameSystem.GameStates.GameOver;
        Bootstrap.Instance.ScoreManager.CheckHigScore();
    }

    private void FixedUpdate()
    {
        if (physics.Collide(transform.position, out var newPosition))
        {
            _newPosition = newPosition;
        }
    }
}