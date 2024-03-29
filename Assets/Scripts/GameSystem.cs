﻿using System;
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
        private readonly PoolSystem _poolSystem;
        private readonly Settings _settings;

        public event Action<GameStates, GameStates> GameStatesChange;

        private GameStates _state = GameStates.Menu;
        private float _gameStart;
        public float PlayTime;

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
                        GameOver();
                        break;
                }

                GameStatesChange?.Invoke(_state, old);
            }
        }

        private void GameOver()
        {
            _bgm.Play(BgmSystem.Music.Pause);
        }

        public GameSystem(BgmSystem bgm, PoolSystem poolSystem, Settings settings)
        {
            _bgm = bgm;
            _poolSystem = poolSystem;
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
            _gameStart = GameTime.time;
            _bgm.Play(BgmSystem.Music.Gameplay);
            PlayTime = 0;

            _poolSystem.Initiate();

            _settings.ScrollSpeed = _settings.MinScrollSpeed;
        }

        public void PlayerDied()
        {
            
            State = GameStates.GameOver;
            
        }

        public void Update()
        {
            _settings.ScrollSpeed += Math.Min(_settings.MaxScrollSpeed - _settings.ScrollSpeed, GameTime.deltaTime * _settings.Acceleration);
            if (State== GameStates.GameStart)
            {
                PlayTime += Time.deltaTime;
            }
        }
    }
}