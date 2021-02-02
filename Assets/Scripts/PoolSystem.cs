using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;

namespace DefaultNamespace
{
    public class PoolSystem
    {
        public int LoadedPrefabs;
        public Queue<GameObject> CurrentPool = new Queue<GameObject>();
        public List<GameObject> AllPrefabs;
        public Queue<GameObject> ShuffleQueue = new Queue<GameObject>();

        public void Shuffle()
        {
            var rng = new Random();
            int n = AllPrefabs.Count;
            for (int i = 0; i < 4; i++)
            {
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    var value = AllPrefabs[k];
                    AllPrefabs[k] = AllPrefabs[n];
                    AllPrefabs[n] = value;
                }
            }
            foreach (var prefab in AllPrefabs)
            {
                ShuffleQueue.Enqueue(prefab);
            }
        }

        private void FillCurrent()
        {
            for (int i = 0; i < LoadedPrefabs; i++)
            {
                var obj = ShuffleQueue.Dequeue();
                obj.SetActive(true);
                CurrentPool.Enqueue(obj);
            }
        }

        public void RollPool()
        {
            var obj = CurrentPool.Dequeue();
            obj.SetActive(false);
            ShuffleQueue.Enqueue(obj);
        }
    }
}