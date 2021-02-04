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
        if (_physics.Move(transform.position, Vector3Int.right, out var newPosition))
        {
            transform.position = new Vector3(transform.position.x +
                                             Mathf.Min(Mathf.Abs(newPosition.x),
                                                 GameTime.deltaTime * _settings.ScrollSpeed * -2) *
                                             Mathf.Sign(newPosition.x), transform.position.y, 0);
        }
        else
        {
            gameObject.SetActive(false);
        }
        // transform.position += -_settings.ScrollDirection * (Time.deltaTime * _settings.ScrollSpeed*2);
        // if (transform.position.x > 50)
        // {
        //     gameObject.SetActive(false);
        // }
    }

    
}
