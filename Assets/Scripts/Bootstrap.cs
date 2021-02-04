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
        
        [HideInInspector]
        public InputListener inpListener;
        
        public PoolSystem PoolSystem;
        public GameSystem GameSystem;
        public BgmSystem BgmSystem;
        public ScoreManager ScoreManager = new ScoreManager();
        public Settings Settings;
        public GameTime GameTime;
        
        [Space]
        public PoolSystem.Block[] blocks;
        
        private void Awake()
        {
            if (_instance == null)
                _instance = this;

            inpListener = FindObjectOfType<InputListener>();
            BgmSystem = new BgmSystem();
            GameTime = new GameTime();
            PoolSystem = new PoolSystem(Settings, blocks);
            GameSystem = new GameSystem(BgmSystem, PoolSystem, Settings);
        }

        private void Update()
        {
            GameTime.Update();
        }

        private void FixedUpdate()
        {
            GameTime.FixedUpdate();
        }
    }
}