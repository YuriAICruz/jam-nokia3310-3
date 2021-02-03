using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Bootstrap : MonoBehaviour
    {
        public static Bootstrap Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = FindObjectOfType<Bootstrap>().gameObject;
                    if (obj == null)
                    {
                        obj = new GameObject("Bootstrap");
                        obj.AddComponent<Bootstrap>();
                    }
                    _instance = obj.GetComponent<Bootstrap>();
                }

                return _instance;
            }
        }

        private static Bootstrap _instance;


        public PoolSystem PoolSystem = new PoolSystem();
        public GameSystem GameSystem;
        public BgmSystem BgmSystem;
        public ScoreManager ScoreManager = new ScoreManager();
        public Settings Settings;
        
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            
            BgmSystem = new BgmSystem();
            GameSystem = new GameSystem(BgmSystem, Settings);
        }
    }
}