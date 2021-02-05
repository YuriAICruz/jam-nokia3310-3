using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Physics = DefaultNamespace.Physics;

public class Fire : MonoBehaviour
{

    private Physics _physics;

    private Settings _settings;
    // Start is called before the first frame update
    void Start()
    {
        _physics = new Physics();
        _settings = Bootstrap.Instance.Settings;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x > 50||_physics.Collide(transform.position+Vector3.forward,  out var newPosition))
        {
            gameObject.SetActive(false);
        }
        else
        {
            var _position = transform.position= newPosition;
            _position += -_settings.ScrollDirection * (GameTime.deltaTime * _settings.ScrollSpeed*2);
            transform.position = _physics.SetPosition(_position);
        }
    }

    
}
