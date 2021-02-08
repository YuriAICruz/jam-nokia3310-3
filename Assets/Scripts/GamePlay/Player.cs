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
    private bool _jumping => _lastJump <= _settings.JumpDuration;
    private int _noCollisionSteps;

    private float _gravityVelocity;
    private int _cell;

    private void Awake()
    {
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
            _settings.ResetGravity();
            transform.position = _lastPosition = _newPosition = _startPosition;
            transform.localScale = new Vector3(1, -_settings.Gravity.y, 1);
        }
    }

    private void Update()
    {
        if (_gameSystem.State != GameSystem.GameStates.GameStart) return;

        _lastJump += GameTime.deltaTime;

        if (!grounded && !_jumping)
        {
            _gravityVelocity += GameTime.deltaTime * _settings.GravitySpeed;
        }


        var dir = -_lastPosition + _newPosition;
        var pos = _lastPosition =
            new Vector3(
                _lastPosition.x + Mathf.Min(
                    Mathf.Abs(dir.x), GameTime.deltaTime * _settings.ScrollSpeed * 2) * Mathf.Sign(dir.x),
                _lastPosition.y + Mathf.Min(
                    Mathf.Abs(dir.y), _jumping ? GameTime.deltaTime * _settings.JumpSpeed : _gravityVelocity) *
                Mathf.Sign(dir.y),
                0f
            );

        transform.position = physics.SetPosition(pos);

        transform.localScale = new Vector3(1, -_settings.Gravity.y, 1);
        // if (transform.position.y <= 1 && transform.position.y >= 1)
        //     transform.localScale = new Vector3(1, -_settings.Gravity.y, 1);

        CheckDeath();
    }

    private void CheckDeath()
    {
        if (transform.position.x < _settings.deathPosition.x)
            _gameSystem.PlayerDied();

        if (Mathf.Abs(transform.position.y) > _settings.deathPosition.y)
            _gameSystem.PlayerDied();
    }

    private void FixedUpdate()
    {
        if (_gameSystem.State != GameSystem.GameStates.GameStart) return;

        _cell = 6;

        if (physics.Collide(transform.position, out var newPosition))
        {
            var y = _newPosition.y;
            _newPosition = newPosition;

            if (_jumping)
            {
                _newPosition.y = y;
            }

            _noCollisionSteps = 0;
        }
        else
        {
            _noCollisionSteps++;

            if (_noCollisionSteps > 5)
                _newPosition.x += Mathf.Min(_startPosition.x - _newPosition.x,
                    GameTime.deltaTime * _settings.ReturnSpeed);
        }

        if (_jumping)
        {
            return;
        }

        if (physics.Gravity(transform.position, out var gravPosition) &&
            physics.Gravity(transform.position + Vector3.left * _cell, out var gravPositionb) &&
            physics.Gravity(transform.position + Vector3.left * (_cell * 2), out gravPositionb)
        )
        {
            _newPosition.y = gravPosition.y;
            Ground();
        }
        else
        {
            var dist = physics.GetCellDistance(transform.position);

            if (Mathf.Abs(dist.y) < 2)
                grounded = true;
        }
    }

    private void ChangeGravity()
    {
        if (!grounded || _gameSystem.State != GameSystem.GameStates.GameStart)
        {
            return;
        }

        _settings.InvertGravity();
        Ground();
    }

    private void Jump()
    {
        if (!grounded || _gameSystem.State != GameSystem.GameStates.GameStart) return;

        if (physics.Move(transform.position - Vector3.right * _cell * 2, -_settings.Gravity * 2, out var jumpPosition))
        {
            _newPosition.y = jumpPosition.y;
            _lastJump = 0f;
            Ground();
        }
        else
        {
            //_newPosition.y += -_settings.Gravity.y * 2 * _cell;
        }
    }

    private void Ground()
    {
        grounded = false;
        _gravityVelocity = 0;
    }

    private void CancelJump()
    {
        _lastJump = 999f;
    }
}