using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    [Serializable]
    public class Settings
    {
        [Header("Tilemaps")] public Tilemap CollidersMap;
        public Tilemap GraphicsMap;

        [Header("Player")] public float StepSize = 1;
        public float JumpHeith;
        public float JumpDuration;

        [Header("Scroll")] 
        public float ScrollSpeed;
        public float MaxScrollSpeed;
        public float MinScrollSpeed;
        public Vector3 ScrollDirection;

        [Header("Music")] public AudioClip MenuSong;
        public AudioClip GameplaySong;
        
        public Vector3Int Gravity = new Vector3Int(0,-1,0);

        public void InvertGravity()
        {
            Gravity *= -1;
        }
    }
}