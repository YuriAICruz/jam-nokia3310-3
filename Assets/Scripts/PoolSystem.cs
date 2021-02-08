using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GamePlay;
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
            Graphics = 1
        }

        [Serializable]
        public class Block
        {
            [Serializable]
            public class Data
            {
                public int cellSize;
                public TileType type;
                public Vector3Int[] positions;
                public string[] tiles;
                private TileBase[] _bases;

                public Data()
                {
                }

                public Data(int cellSize, TileType type, Vector3Int[] positions, string[] tiles)
                {
                    this.cellSize = cellSize;
                    this.type = type;
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

                    var size = (84 / cellSize);

                    for (int i = 0; i < positions.Length; i++)
                    {
                        pos[i] = new Vector3Int(pos[i].x + size * offset, pos[i].y, pos[i].z);
                    }

                    return pos.ToArray();
                }
            }

            public Data[] data;

            public Block()
            {
            }

            public Block(Data[] data)
            {
                this.data = new Data[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    this.data[i] = new Data(data[i].cellSize, data[i].type, data[i].positions, data[i].tiles);
                }
            }
        }

        private readonly Settings _settings;
        private readonly Block[] _blocks;
        private readonly Player _player;
        private readonly Mover _tilesMover;

        public int LoadedPrefabs;
        public Queue<GameObject> CurrentPool = new Queue<GameObject>();
        public List<GameObject> AllPrefabs = new List<GameObject>();
        public Queue<GameObject> ShuffleQueue = new Queue<GameObject>();
        private int _last = -1;

        private readonly Vector3 _anchor;

        public PoolSystem(Settings settings, Block[] blocks, Player player, Mover tilesMover)
        {
            _settings = settings;
            _blocks = blocks;
            _player = player;
            _tilesMover = tilesMover;

            _anchor = new Vector3((int) ((_settings.ScreenCollidersCellSize / 2 + 1) * 6), 0, 0);
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
            _tilesMover.Reset(-6);
            _settings.CollidersMap.ClearAllTiles();
            _settings.GraphicsMap.ClearAllTiles();
        }

        private void Populate()
        {
            int position = 0;
            int n = _blocks.Length;
            var indexes = new List<int>();

            if (_last >= 0)
            {
                Reset();
                SetTiles(_last, 0);
                position = 1;

                for (int i = 0; i < 3; i++)
                {
                    indexes.Add(i);
                }
            }

            for (; position < n; position++)
            {
                var index = position % _blocks.Length;

                if (position < 3)
                {
                    index = position;
                }
                else if (indexes.Count < _blocks.Length)
                {
                    index = UnityEngine.Random.Range(0, _blocks.Length);

                    while (indexes.Contains(index))
                    {
                        index = UnityEngine.Random.Range(0, _blocks.Length);
                    }
                }
                else
                {
                    indexes = new List<int>();
                    for (int i = 0; i < 3; i++)
                    {
                        indexes.Add(i);
                    }
                }

                indexes.Add(index);

                _last = index;

                SetTiles(index, position);
            }
        }

        private void SetTiles(int index, int positionInLine)
        {
            var tiles = Enum.GetValues(typeof(TileType));

            for (int j = 0; j < tiles.Length; j++)
            {
                var type = (TileType) tiles.GetValue(j);
                var cols = _blocks[index].data.First(x => x.type == type);
                switch (type)
                {
                    case TileType.Collider:
                        _settings.CollidersMap.SetTiles(cols.GetPositions(positionInLine), cols.GetTiles());
                        break;
                    case TileType.Graphics:
                        _settings.GraphicsMap.SetTiles(cols.GetPositions(positionInLine), cols.GetTiles());
                        break;
                }
            }
        }

        public void Update()
        {
            Debug.DrawRay(_anchor, Vector3.up * 10, Color.red, 5);
            var pos =
                _settings.CollidersMap.WorldToCell(_anchor).x /
                _settings.ScreenCollidersCellSize;
            if (pos >= _blocks.Length - 1)
            {
                Populate();
            }
        }

        public void Initiate()
        {
            _last = -1;
            Reset();
            Populate();
        }
    }
}