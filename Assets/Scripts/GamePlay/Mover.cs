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

        private void Awake()
        {
            _settings = Bootstrap.Instance.Settings;
            _physics = new Physics();

            _position = transform.position;
        }

        private void Update()
        {
            _position += _settings.ScrollDirection * (Time.deltaTime * _settings.ScrollSpeed);
            transform.position = _physics.SetPosition(_position);
        }
    }
}