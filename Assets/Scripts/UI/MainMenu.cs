using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject menu;
        public GameObject creditosPainel;

        public Button bStart;
        public Button credts;
        public Button exitCred;
        
        private void Start()
        {
            bStart.onClick.AddListener(Play);
            credts.onClick.AddListener(OpenCredts);
            exitCred.onClick.AddListener(CloseCredts);
           
        }
        

        private void CloseCredts()
        {
            creditosPainel.SetActive(false);
        }

        private void OpenCredts()
        {
            creditosPainel.SetActive(true);
        }

        private void Play()
        {
            Bootstrap.Instance.GameSystem.state = GameSystem.GameStates.GameStart;
            menu.SetActive(false);
        }
        
    }
}