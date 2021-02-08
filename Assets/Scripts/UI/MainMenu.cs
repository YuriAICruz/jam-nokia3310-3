using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Serializable]
        public class WindowData
        {
            public GameObject Object;
            public Navigation Navigation;
        }

        public WindowData Menu;
        public WindowData Credits;
        public WindowData PauseWindow;
        public WindowData GameOver;

        public Button StartGame;
        public Button OpenCredits;
        public Button ExitCredits;
        public Button UnPause;
        public Button Restart;
        public Button ToMenu;

        public Text ScoreText;
        
        Navigation _currentPanel;

        private GameSystem _system;
        private InputListener _inputs;
        private GameTime _gameTime;

        private void Start()
        {
            _inputs = Bootstrap.Instance.inpListener;
            _inputs.Pause += Pause;
            _inputs.Navigate += Navigate;
            _inputs.Accept += Accept;

            _gameTime = Bootstrap.Instance.GameTime;

            StartGame.onClick.AddListener(Play);
            OpenCredits.onClick.AddListener(OpenCredts);
            ExitCredits.onClick.AddListener(OpenMenu);
            UnPause.onClick.AddListener(Pause);
            Restart.onClick.AddListener(Play);
            ToMenu.onClick.AddListener(OpenMenu);

            _system = Bootstrap.Instance.GameSystem;

            _system.GameStatesChange += StateChanged;

            SetMenuWindow(true);
            SetCreditsWindow(false);
        }

        private void Accept()
        {
            if (!_currentPanel) return;
            
            _currentPanel.Selection.onClick.Invoke();
        }

        private void Update()
        {
            if (_currentPanel && !_currentPanel.IsSelected)
            {
                _currentPanel.Selection.Select();
            }
        }

        private void SetMenuWindow(bool value)
        {
            Menu.Object.SetActive(value);
            UpdateNavigation(Menu.Navigation);
        }

        private void SetCreditsWindow(bool value)
        {
            Credits.Object.SetActive(value);
            if (value)
                UpdateNavigation(Credits.Navigation);
        }

        private void SetPauseWindow(bool value)
        {
            if (_system.State != GameSystem.GameStates.GameStart) return;

            PauseWindow.Object.SetActive(value);
            if (value)
                UpdateNavigation(PauseWindow.Navigation);
        }

        private void SetGameOverWindow(bool value)
        {
            GameOver.Object.SetActive(value);
            if (value)
                UpdateNavigation(GameOver.Navigation);
        }

        private void StateChanged(GameSystem.GameStates state, GameSystem.GameStates oldState)
        {
            switch (state)
            {
                case GameSystem.GameStates.Menu:
                    SetMenuWindow(true);
                    SetCreditsWindow(false);
                    SetGameOverWindow(false);
                    break;
                case GameSystem.GameStates.GameStart:
                    SetMenuWindow(false);
                    SetGameOverWindow(false);
                    break;
                case GameSystem.GameStates.GameOver:
                    SetGameOverWindow(true);
                    ScoreText.text = _system.PlayTime.ToString("0000");
                    break;
                case GameSystem.GameStates.Credits:
                    SetMenuWindow(false);
                    SetCreditsWindow(true);
                    break;
            }
        }


        private void OpenMenu()
        {
            _system.State = GameSystem.GameStates.Menu;
        }

        private void OpenCredts()
        {
            _system.State = GameSystem.GameStates.Credits;
        }

        private void Play()
        {
            _system.State = GameSystem.GameStates.GameStart;
            _currentPanel = null;
        }

        private void UpdateNavigation(Navigation navigation)
        {
            _currentPanel = navigation;

            _currentPanel.Selection.Select();
        }

        private void Navigate(Vector2Int direction)
        {
            if (!_currentPanel) return;

            if (direction.x > 0 && _currentPanel.Right)
            {
                UpdateNavigation(_currentPanel.Right);
            }
            else if (direction.x < 0 && _currentPanel.Left)
            {
                UpdateNavigation(_currentPanel.Left);
            }
        }

        private void Pause()
        {
            if (_gameTime.paused)
            {
                _gameTime.UnPause();
                SetPauseWindow(false);
                _currentPanel = null;
            }
            else
            {
                _gameTime.Pause();
                SetPauseWindow(true);
            }
        }
    }
}