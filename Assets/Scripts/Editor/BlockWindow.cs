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

        public int[] cellSizes;
        public List<Vector3Int>[] positions;
        public List<string>[] tiles;

        private string _fileName;
        private Array _types;
        private Bootstrap _bootstrap;
        
        private string[] _currentTile;

        [MenuItem("Game/Tileset Block")]
        static void Init()
        {
            _window = (BlockWindow) GetWindow(typeof(BlockWindow));
            _window.Show();
        }

        private void OnGUI()
        {
            if (_types == null || cellSizes == null || positions == null || tiles == null)
            {
                _bootstrap = FindObjectOfType<Bootstrap>();
                _types = Enum.GetValues(typeof(PoolSystem.TileType));
                cellSizes = new int[_types.Length];
                positions = new List<Vector3Int>[_types.Length];
                tiles = new List<string>[_types.Length];
            }
            if (_currentTile == null)
            {
                _currentTile = new string[_types.Length];
                for (int i = 0; i < _types.Length; i++)
                {
                    _currentTile[i] = "Tiles/";
                }
            }

            type = (PoolSystem.TileType) EditorGUILayout.EnumPopup(type);

            ShowGrid(type);
        }

        private void ShowGrid(PoolSystem.TileType type)
        {
            var typeIndex = (int) type;

            if (positions[typeIndex] == null || tiles[typeIndex] == null)
            {
                positions[typeIndex] = new List<Vector3Int>();
                tiles[typeIndex] = new List<string>();
            }

            var reference = $"Grid - {type}";

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

            cellSizes[typeIndex] = (int) grid.cellSize.x;

            var size = new Vector2Int(84 / (int) grid.cellSize.x + 1, 42 / (int) grid.cellSize.y + 1);

            var selectedStyle = new GUIStyle(GUI.skin.button);
            selectedStyle.normal.textColor = new Color(0, 1f, 0.6f);
            selectedStyle.hover.textColor = new Color(1f, 0.2f, 0.2f);
            selectedStyle.fontSize = 10;

            var unSelectedStyle = new GUIStyle(GUI.skin.button);
            unSelectedStyle.normal.textColor = new Color(0.4f, 0.4f, 0.4f);
            unSelectedStyle.hover.textColor = new Color(0.0f, 0.6f, 0.9f);
            selectedStyle.fontSize = 10;

            EditorGUILayout.BeginHorizontal();
            _currentTile[typeIndex] = EditorGUILayout.TextField("Tile Resource", _currentTile[typeIndex]);

            if (_currentTile == null || Resources.Load<TileBase>(_currentTile[typeIndex]) == null)
            {
                EditorGUILayout.LabelField("Null Tile, write a valid Resource Tile");
                return;
            }

            if (GUILayout.Button("Reset"))
            {
                positions[typeIndex] = new List<Vector3Int>();
                tiles[typeIndex] = new List<string>();
                _fileName = "";
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            for (int x = -size.x / 2; x <= size.x / 2; x++)
            {
                EditorGUILayout.BeginVertical();
                for (int y = size.y / 2; y >= -(size.y / 2+1); y--)
                {
                    var pos = new Vector3Int(x, y, 0);
                    var selected = positions[typeIndex].Contains(pos);
                    var index = positions[typeIndex].FindIndex(v => v == pos);

                    if (GUILayout.Button(selected ? tiles[typeIndex][index].Split('/').Last() : $"{x:00}\n{y:00}",
                        selected ? selectedStyle : unSelectedStyle, GUILayout.Width(32), GUILayout.Height(32)))
                    {
                        if (selected)
                        {
                            positions[typeIndex].RemoveAt(index);
                            tiles[typeIndex].RemoveAt(index);
                        }
                        else
                        {
                            positions[typeIndex].Add(pos);
                            tiles[typeIndex].Add(_currentTile[typeIndex]);
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            _fileName = EditorGUILayout.TextField("Name", _fileName);

            EditorGUILayout.BeginHorizontal();

            if (!CanSave())
            {
                EditorGUILayout.LabelField("Populate all grid types to save");
            }
            else if (!string.IsNullOrEmpty(_fileName))
            {
                var path = Application.dataPath + "/Resources/Blocks/";
                var file = path + _fileName + ".json";
                var exists = File.Exists(file);
                if (GUILayout.Button(exists ? "Override Save" : "Save"))
                {
                    for (int i = 0; i < tiles[typeIndex].Count; i++)
                    {
                        if (string.IsNullOrEmpty(tiles[typeIndex][i]))
                        {
                            positions[typeIndex].RemoveAt(i);
                            tiles[typeIndex].RemoveAt(i);
                        }
                    }

                    var data = new PoolSystem.Block.Data[_types.Length];

                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = new PoolSystem.Block.Data(cellSizes[i], (PoolSystem.TileType) _types.GetValue(i),
                            positions[i].ToArray(), tiles[i].ToArray());
                    }

                    File.WriteAllText(file, JsonConvert.SerializeObject(new PoolSystem.Block(data)));

                    AssetDatabase.Refresh();
                }
            }

            if (GUILayout.Button("Load"))
            {
                string path = EditorUtility.OpenFilePanel("Select File", "", "json");

                EditorGUILayout.LabelField(path);

                var blocks = JsonConvert.DeserializeObject<PoolSystem.Block>(File.ReadAllText(path));

                cellSizes = new int[_types.Length];
                positions = new List<Vector3Int>[_types.Length];
                tiles = new List<string>[_types.Length];

                for (int i = 0; i < _types.Length; i++)
                {
                    var d = blocks.data.First(x => x.type == (PoolSystem.TileType) _types.GetValue(i));
                    positions[i] = d.positions.ToList();
                    tiles[i] = d.tiles.ToList();
                }

                _fileName = "";
            }

            EditorGUILayout.EndHorizontal();
        }

        private bool CanSave()
        {
            for (int i = 0; i < _types.Length; i++)
            {
                if (positions[i] == null || positions[i].Count == 0)
                    return false;

                if (tiles[i] == null || tiles[i].Count == 0)
                    return false;
            }

            return true;
        }

        private static void NotFound(string reference)
        {
            EditorGUILayout.LabelField($"Not found \"{reference}\", create a Grid with this name");
        }
    }
}