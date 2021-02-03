using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    [Serializable]
    public class Settings
    {
        [Header("Tilemaps")]
        public Tilemap CollidersMap;

        [Header("Player")]
        public float StepSize = 1;
        public float JumpSpeed;
        public float JumpDuration;
        
        [Header("Scroll")]
        public float ScrollSpeed;
        public Vector3 ScrollDirection;
    }
}