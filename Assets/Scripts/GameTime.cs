using UnityEngine;

namespace DefaultNamespace
{
    public class GameTime
    {
        public static float deltaTime => Time.deltaTime;
        public static float fixedDeltaTime => Time.fixedDeltaTime * _scale;

        static float _time;
        static float _fixedTime;
        public static float time => _time;
        public static float fixedTime  => _fixedTime;

        public static bool Paused { get; private set; }
        public bool paused => Paused;

        private static float _scale = 1;

        public void Pause()
        {
            Paused = true;
            _scale = 0;
        }

        public void UnPause()
        {
            Paused = false;
            _scale = 1;
        }

        public void Update()
        {
            _time += deltaTime;
        }

        public void FixedUpdate()
        {
            _fixedTime += fixedDeltaTime;
        }
    }
}