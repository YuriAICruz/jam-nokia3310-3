using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class GameSystem
    {
        public enum GameStates
        {
            Menu,
            GameStart,
            GameOver,
            Credits
        }

        private readonly BgmSystem _bgm;
        private readonly Settings _settings;

        public event Action<GameStates, GameStates> GameStatesChange;

        private GameStates _state = GameStates.Menu;
        private float _gameStart;

        public GameStates State
        {
            get { return _state; }
            set
            {
                var old = _state;
                _state = value;
                switch (_state)
                {
                    case GameStates.Menu:
                        Menu();
                        break;
                    case GameStates.GameStart:
                        StartGame();
                        break;
                    case GameStates.GameOver:
                        break;
                }

                GameStatesChange?.Invoke(_state, old);
            }
        }

        public GameSystem(BgmSystem bgm, Settings settings)
        {
            _bgm = bgm;
            _settings = settings;

            Menu();
        }

        private void Menu()
        {
            _bgm.Play(BgmSystem.Music.Menu);
            _settings.ScrollSpeed = 0;
        }

        private void StartGame()
        {
            _gameStart = Time.time;
            _bgm.Play(BgmSystem.Music.Gameplay);

            _settings.ScrollSpeed = _settings.MinScrollSpeed;
        }
    }
}