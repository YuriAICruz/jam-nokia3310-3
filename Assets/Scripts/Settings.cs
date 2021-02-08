using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    [Serializable]
    public class Settings
    {
        [Header("Tilemaps")] 
        public int ScreenCollidersCellSize;
        public Tilemap CollidersMap;
        public Tilemap GraphicsMap;

        [Header("Player")] public float StepSize = 1;
        public float ReturnSpeed;
        public float JumpSpeed;
        public float TranslationSpeed = 10;
        public float JumpDuration;
        public float GravitySpeed;

        [Header("Scroll")] 
        public float ScrollSpeed;
        public float MaxScrollSpeed;
        public float MinScrollSpeed;
        public Vector3 ScrollDirection;
        public float Acceleration;

        [Header("Music")] public AudioClip MenuSong;
        public AudioClip GameplaySong;

        public Vector3Int Gravity = Vector3Int.down;
        
        [Header("Death")]
        public Vector2Int deathPosition = new Vector2Int(-14, 8);


        public void InvertGravity()
        {
            Gravity *= -1;
        }

        public void ResetGravity()
        {
            Gravity = Vector3Int.down;
        }
    }
}