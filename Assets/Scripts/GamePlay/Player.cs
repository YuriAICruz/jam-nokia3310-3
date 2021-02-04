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
    private Settings _settings;
    private InputListener _inputListener;
    public Physics physics;
    private Vector3 _newPosition;
    private Vector3 _lastPosition;
    private GameSystem _gameSystem;
    private Vector3 _startPosition;
    public bool grounded;
    private float _lastJump;
    private float _baseX;
    private int _noCollisionSteps;

    void Awake()
    {
        _baseX = transform.position.x;
        Instance = this;
        _settings = Bootstrap.Instance.Settings;
        _inputListener = Bootstrap.Instance.inpListener;
        physics = new Physics();

        _inputListener.GravityChange += ChangeGravity;
        _inputListener.Jump += Jump;
        _inputListener.CancelJump += CancelJump;

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
        _lastJump += Time.deltaTime;

        var pos = _lastPosition =
            new Vector3(x: Mathf.Lerp(_lastPosition.x, _newPosition.x, Time.deltaTime * _settings.TranslationSpeed),
                Mathf.Lerp(_lastPosition.y, _newPosition.y, Time.deltaTime * _settings.GravitySpeed), 0f);

        transform.position = physics.SetPosition(pos);

        if (transform.position.y <= 1 && transform.position.y >= 1)
            transform.localScale = new Vector3(1, -_settings.Gravity.y, 1);
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
        var cell = 6;
        Debug.DrawRay(transform.position + Vector3.left * cell, Vector3.up * 10, Color.red);
        if (physics.Collide(transform.position, out var newPosition))
        {
            _newPosition = newPosition;
            _noCollisionSteps = 0;
        }
        else
        {
            _noCollisionSteps++;
            if (_noCollisionSteps > 5)
                _newPosition.x += Mathf.Min(_baseX - _newPosition.x,  Time.deltaTime * _settings.ReturnSpeed);
        }

        if (_lastJump <= _settings.JumpDuration)
        {
            return;
        }

        if (physics.Gravity(transform.position, out var gravPosition) &&
            physics.Gravity(transform.position + Vector3.left * cell, out var gravPositionb) &&
            physics.Gravity(transform.position + Vector3.left * cell * 2, out gravPositionb)
        )
        {
            _newPosition.y = gravPosition.y;
            grounded = false;
        }
        else
        {
            var dist = physics.GetCellDistance(transform.position);

            if (dist.y < 1)
                grounded = true;
        }
    }

    private void ChangeGravity()
    {
        if (!grounded)
        {
            return;
        }

        _settings.InvertGravity();
        grounded = false;
    }

    private void Jump()
    {
        if (grounded && physics.Move(transform.position, -_settings.Gravity * 2, out var jumpPosition))
        {
            _newPosition.y = jumpPosition.y;
            _lastJump = 0f;
            grounded = false;
        }
    }

    private void CancelJump()
    {
        _lastJump = 999f;
    }
}