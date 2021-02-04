using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject[] menu;
        public GameObject[] creditosPainel;

        public Button StartGame;
        public Button credts;
        public Button exitCred;

        private Button _current;

        private GameSystem _system;
        private InputListener _inputs;

        private void Start()
        {
            _inputs = Bootstrap.Instance.inpListener;
            _inputs.Pause += Pause;
            _inputs.Navigate += Navigate;
            
            StartGame.onClick.AddListener(Play);
            credts.onClick.AddListener(OpenCredts);
            exitCred.onClick.AddListener(CloseCredts);

            _system = Bootstrap.Instance.GameSystem;

            _system.GameStatesChange += StateChanged;
            
            SetMenuWindow(true);
            SetCreditsWindow(false);
        }

        private void Navigate(Vector2Int direction)
        {
            
        }

        private void Pause()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _system.State = GameSystem.GameStates.Menu;
            }
        }

        private void SetMenuWindow(bool value)
        {
            for (int i = 0; i < menu.Length; i++)
            {
                menu[i].SetActive(value);
            }
        }

        private void SetCreditsWindow(bool value)
        {
            for (int i = 0; i < creditosPainel.Length; i++)
            {
                creditosPainel[i].SetActive(value);
            }
        }

        private void StateChanged(GameSystem.GameStates state, GameSystem.GameStates oldState)
        {
            switch (state)
            {
                case GameSystem.GameStates.Menu:
                    SetMenuWindow(true);
                    SetCreditsWindow(false);
                    break;
                case GameSystem.GameStates.GameStart:
                    SetMenuWindow(false);
                    break;
                case GameSystem.GameStates.GameOver:
                    break;
                case GameSystem.GameStates.Credits:
                    SetMenuWindow(false);
                    SetCreditsWindow(true);
                    break;
            }
        }


        private void CloseCredts()
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
        }
    }
}