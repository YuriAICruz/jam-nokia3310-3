using System;
using System.Diagnostics;
using UnityEngine;

namespace DefaultNamespace.GamePlay
{
    public class Mover : MonoBehaviour
    {
        private Settings _settings;
        private Physics _physics;
        private Vector3 _position;
        private Vector3 _startPosition;
        private GameSystem _gameSystem;

        private void Awake()
        {
            _settings = Bootstrap.Instance.Settings;
            _physics = new Physics();

            _position = _startPosition = transform.position;

            _gameSystem = Bootstrap.Instance.GameSystem;
            _gameSystem.GameStatesChange += GameStatesChange;
        }

        private void GameStatesChange(GameSystem.GameStates state, GameSystem.GameStates oldState)
        {
            if (state == GameSystem.GameStates.GameStart)
            {
                _position = transform.position = _startPosition;
            }
        }

        private void Update()
        {
            _position += _settings.ScrollDirection * (Time.deltaTime * _settings.ScrollSpeed);
            transform.position = _physics.SetPosition(_position);
        }
    }
}