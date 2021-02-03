using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class PoolSystem
    {
        public enum TileType
        {
            Collider = 0,
            Graphics = 1,
            Physics = 2
        }

        [Serializable]
        public class Block
        {
            public int cellSize;
            public Vector3Int[] positions;
            public string[] tiles;
            private TileBase[] _bases;

            public Block()
            {
            }

            public Block(int cellSize, Vector3Int[] positions, string[] tiles)
            {
                this.cellSize = cellSize;
                this.positions = positions;
                this.tiles = tiles;
            }

            public TileBase[] GetTiles()
            {
                if (_bases != null)
                {
                    return _bases;
                }

                _bases = new TileBase[tiles.Length];

                for (int i = 0; i < _bases.Length; i++)
                {
                    _bases[i] = Resources.Load<TileBase>(tiles[i]);
                }

                return _bases;
            }

            public Vector3Int[] GetPositions(int offset)
            {
                var pos = positions.ToList();

                var size = 84 / (int) cellSize + 1;
                for (int i = 0; i < positions.Length; i++)
                {
                    pos[i] = new Vector3Int(pos[i].x + size * offset, pos[i].y, pos[i].z);
                }

                return pos.ToArray();
            }
        }

        private readonly Settings _settings;
        private readonly Block[] _blocks;
        public int LoadedPrefabs;
        public Queue<GameObject> CurrentPool = new Queue<GameObject>();
        public List<GameObject> AllPrefabs = new List<GameObject>();
        public Queue<GameObject> ShuffleQueue = new Queue<GameObject>();


        public PoolSystem(Settings settings, Block[] blocks)
        {
            _settings = settings;
            _blocks = blocks;
        }

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

        public void Reset()
        {
            var rnd = UnityEngine.Random.Range(0, 1);
            _settings.CollidersMap.ClearAllTiles();
            _settings.CollidersMap.SetTiles(_blocks[0].GetPositions(0), _blocks[0].GetTiles());
            _settings.CollidersMap.SetTiles(_blocks[1].GetPositions(1), _blocks[0].GetTiles());
        }
    }
}