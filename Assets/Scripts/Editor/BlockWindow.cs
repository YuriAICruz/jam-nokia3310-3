using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Editor
{
    public class BlockWindow : EditorWindow
    {
        private static BlockWindow _window;

        private PoolSystem.TileType type;

        public List<Vector3Int> positions;
        public List<string> tiles;
        private string _currentTile = "Tiles/";
        private string _fileName;
        private int _cellSize;

        [MenuItem("Game/Tileset Block")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            _window = (BlockWindow) GetWindow(typeof(BlockWindow));
            _window.Show();
        }

        void OnGUI()
        {
            type = (PoolSystem.TileType) EditorGUILayout.EnumPopup(type);

            switch (type)
            {
                case PoolSystem.TileType.Collider:
                    ShowGrid("Grid - Colliders");
                    return;
                case PoolSystem.TileType.Graphics:
                    return;
                case PoolSystem.TileType.Physics:
                    return;
            }

            EditorGUILayout.LabelField("Hello");
        }

        private void ShowGrid(string reference)
        {
            if (positions == null || tiles == null)
            {
                positions = new List<Vector3Int>();
                tiles = new List<string>();
            }

            var go = GameObject.Find(reference);

            if (go == null)
            {
                NotFound(reference);
                return;
            }

            var grid = go.GetComponent<Grid>();

            if (grid == null)
            {
                NotFound(reference);
                return;
            }

            _cellSize = (int) grid.cellSize.x;

            var size = new Vector2Int(84 / (int) grid.cellSize.x + 1, 42 / (int) grid.cellSize.y + 1);

            var selectedStyle = new GUIStyle(GUI.skin.button);
            selectedStyle.normal.textColor = new Color(0, 1f, 0.6f);
            selectedStyle.hover.textColor = new Color(1f, 0.2f, 0.2f);

            var unSelectedStyle = new GUIStyle(GUI.skin.button);
            unSelectedStyle.normal.textColor = new Color(0.4f, 0.4f, 0.4f);
            unSelectedStyle.hover.textColor = new Color(0.0f, 0.6f, 0.9f);

            EditorGUILayout.BeginHorizontal();
            _currentTile = EditorGUILayout.TextField("Tile Resource", _currentTile);

            if (_currentTile == null || Resources.Load<TileBase>(_currentTile) == null)
            {
                EditorGUILayout.LabelField("Null Tile, write a valid Resource Tile");
                return;
            }

            if (GUILayout.Button("Reset"))
            {
                positions = new List<Vector3Int>();
                tiles = new List<string>();
                _fileName = "";
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            for (int x = -size.x / 2; x <= size.x / 2; x++)
            {
                EditorGUILayout.BeginVertical();
                for (int y = size.y / 2; y >= -size.y / 2; y--)
                {
                    var pos = new Vector3Int(x, y, 0);
                    var selected = positions.Contains(pos);

                    if (GUILayout.Button($"{x:00}\n{y:00}", selected ? selectedStyle : unSelectedStyle))
                    {
                        if (selected)
                        {
                            var index = positions.FindIndex(v => v == pos);
                            positions.RemoveAt(index);
                            tiles.RemoveAt(index);
                        }
                        else
                        {
                            positions.Add(pos);
                            tiles.Add(_currentTile);
                        }
                    }
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            _fileName = EditorGUILayout.TextField("Name", _fileName);

            EditorGUILayout.BeginHorizontal();

            if (!string.IsNullOrEmpty(_fileName))
            {
                var path = Application.dataPath + "/Resources/Blocks/";
                var file = path + _fileName + ".json";
                var exists = File.Exists(file);
                if (GUILayout.Button(exists ? "Override Save" : "Save"))
                {
                    for (int i = 0; i < tiles.Count; i++)
                    {
                        if (string.IsNullOrEmpty(tiles[i]))
                        {
                            positions.RemoveAt(i);
                            tiles.RemoveAt(i);
                        }
                    }

                    File.WriteAllText(file,
                        JsonConvert.SerializeObject(new PoolSystem.Block(_cellSize, positions.ToArray(), tiles.ToArray()))
                    );

                    AssetDatabase.Refresh();
                }
            }

            if (GUILayout.Button("Load"))
            {
                string path = EditorUtility.OpenFilePanel("Select File", "", "json");

                EditorGUILayout.LabelField(path);

                var blocks = JsonConvert.DeserializeObject<PoolSystem.Block>(File.ReadAllText(path));

                positions = blocks.positions.ToList();
                tiles = blocks.tiles.ToList();
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void NotFound(string reference)
        {
            EditorGUILayout.LabelField($"Not found {reference}, create a Grid with this name");
        }
    }
}