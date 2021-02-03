using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class GameOver : MonoBehaviour
    {
        public GameObject Painel;
        public GameObject MainMenu;

        public Button Menu;
        public Button Restart;

        private void Start()
        {
            Menu.onClick.AddListener(OpenMenu);
            Restart.onClick.AddListener(PlayAgain);
        }

        private void PlayAgain()
        {
            Bootstrap.Instance.GameSystem.State = GameSystem.GameStates.GameStart;
            Painel.SetActive(false);
        }

        private void OpenMenu()
        {
            Bootstrap.Instance.GameSystem.State = GameSystem.GameStates.Menu;
            Painel.SetActive(false);
            MainMenu.SetActive(true);
        }
    }
}