using UnityEngine;

namespace DefaultNamespace
{
    public class BgmSystem
    {
        private AudioSource _player;
        private Settings _settings;

        public enum Music
        {
            Null = 0,
            Menu = 1,
            Gameplay =2
        }

        public BgmSystem()
        {
            _settings = Bootstrap.Instance.Settings;
            _player = Object.Instantiate(Resources.Load<AudioSource>("BgmPlayer"));
        }

        public void Play(Music music)
        {
            _player.Stop();
            switch (music)
            {
                case Music.Null:
                    break;
                case Music.Menu:
                    _player.clip = _settings.MenuSong;
                    _player.Play();
                    break;
                case Music.Gameplay:
                    _player.clip = _settings.GameplaySong;
                    _player.Play();
                    break;
            }
        }
    }
}