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
                Reset();
            }
        }

        private void Update()
        {
            _position += _settings.ScrollDirection * (GameTime.deltaTime * _settings.ScrollSpeed);
            transform.position = _physics.SetPosition(_position);
        }

        public void Reset(float offsset = 0)
        {
            if (offsset == 0)
            {
                _position = transform.position = _startPosition;
                return;
            }
            
            _position  = _startPosition;
            _position += Vector3.right * offsset;
            transform.position = _position;
        }
    }
}