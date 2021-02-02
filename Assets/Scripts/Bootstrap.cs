﻿using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bootstrap : MonoBehaviour
    {
        public static Bootstrap Instance
        {
            get
            {
                if(_instance ==null){
                    var obj = new GameObject("Bootstrap");
                    obj.AddComponent<Bootstrap>();
                }

                return _instance;
            }
        }

        private static Bootstrap _instance ;


        public PoolSystem PoolSystem = new PoolSystem();
        public GameSystem GameSystem = new GameSystem();
        public ScoreManager ScoreManager = new ScoreManager();

        private void Awake()
        {
            _instance = this;
        }
        
    }
}