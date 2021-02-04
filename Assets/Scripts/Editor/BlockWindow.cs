using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject.ReflectionBaking.Mono.Cecil;
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
        private int _colliderIndex;

        [MenuItem("Game/Tileset Block")]
        static void Init()
        {
            _window = (BlockWindow) GetWindow(typeof(BlockWindow));
            _window.Show();
        }

        private void OnGUI()
        {
            if (_window == null)
                _window = (BlockWindow) GetWindow(typeof(BlockWindow));

            if (_types == null || cellSizes == null || positions == null || tiles == null)
            {
                _bootstrap = FindObjectOfType<Bootstrap>();
                _types = Enum.GetValues(typeof(PoolSystem.TileType));
                cellSizes = new int[_types.Length];
                for (int i = 0; i < cellSizes.Length; i++)
                {
                    cellSizes[i] = -1;
                }

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

            if (_colliderIndex < 0)
            {
                for (int i = 0; i < _types.Length; i++)
                {
                    if ((PoolSystem.TileType) _types.GetValue(i) == PoolSystem.TileType.Collider)
                    {
                        _colliderIndex = i;
                    }
                }
            }

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

            _window.minSize = new Vector2(240, 320);
            //_window.minSize = new Vector2(480, 640);

            var windowSize = new Vector2Int(
                Mathf.CeilToInt(_window.position.width - 40),
                Mathf.CeilToInt(_window.position.height - 120)
            );
            var size = new Vector2Int(84 / (int) grid.cellSize.x + 1, 42 / (int) grid.cellSize.y + 1);

            var buttonSize = new Vector2Int(
                Mathf.FloorToInt(windowSize.x / (float) size.x - 1),
                Mathf.FloorToInt(windowSize.y / (float) (size.y + 1))
            );

            var yTarget = size.y / 2;

            if (type != PoolSystem.TileType.Collider)
            {
                yTarget += 1;
            }

            EditorGUILayout.BeginHorizontal();

            if (_currentTile.Length <= typeIndex)
            {
                NotFound("Index Out of Range");
                return;
            }
            
            _currentTile[typeIndex] = EditorGUILayout.TextField("Tile Resource", _currentTile[typeIndex]);

            if (_currentTile == null || Resources.Load<Tile>(_currentTile[typeIndex]) == null)
            {
                EditorGUILayout.LabelField("Null Tile, write a valid Resource Tile");
                return;
            }

            if (GUILayout.Button("Reset"))
            {
                positions[typeIndex] = new List<Vector3Int>();
                tiles[typeIndex] = new List<string>();
                cellSizes[typeIndex] = -1;
                _fileName = "";
            }

            EditorGUILayout.EndHorizontal();

            var selectedStyle = new GUIStyle(GUI.skin.button);
            selectedStyle.normal.textColor = new Color(0, 1f, 0.6f);
            selectedStyle.hover.textColor = new Color(1f, 0.2f, 0.2f);
            selectedStyle.fontSize = 10;

            var unSelectedStyle = new GUIStyle(GUI.skin.button);
            unSelectedStyle.normal.textColor = new Color(0.6f, 0.6f, 0.6f);
            unSelectedStyle.hover.textColor = new Color(0.0f, 0.6f, 0.9f);
            unSelectedStyle.fontSize = 8;

            var hasCollider = new GUIStyle(GUI.skin.button);
            hasCollider.normal.textColor = new Color(0.8f, 0.5f, 0.4f);
            hasCollider.hover.textColor = new Color(0.0f, 0.9f, 0.5f);
            hasCollider.fontSize = 8;

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int x = -size.x / 2; x <= size.x / 2; x++)
            {
                EditorGUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                for (int y = size.y / 2; y >= -yTarget; y--)
                {
                    var pos = new Vector3Int(x, y, 0);
                    var selected = positions[typeIndex].Contains(pos);
                    var index = positions[typeIndex].FindIndex(v => v == pos);

                    var onCollider = type != PoolSystem.TileType.Collider &&
                                     positions[_colliderIndex] != null &&
                                     cellSizes[_colliderIndex] > 0 &&
                                     positions[_colliderIndex].Contains(
                                         new Vector3Int(
                                             Mathf.FloorToInt(
                                                 pos.x * (cellSizes[typeIndex] / (float) cellSizes[_colliderIndex])),
                                             Mathf.CeilToInt(
                                                 pos.y * (cellSizes[typeIndex] / (float) cellSizes[_colliderIndex])),
                                             Mathf.CeilToInt(
                                                 pos.z * (cellSizes[typeIndex] / (float) cellSizes[_colliderIndex]))
                                         )
                                     );

                    var text = selected ? "" : $"{x:00}\n{y:00}";
                    var ctn = new GUIContent(text);
                    var rect = GUILayoutUtility.GetRect(
                        ctn,
                        selected ? selectedStyle : onCollider ? hasCollider : unSelectedStyle,
                        GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)
                    );
                    var removed = false;

                    if (GUI.Button(rect, text, selected ? selectedStyle : onCollider ? hasCollider : unSelectedStyle))
                    {
                        if (selected)
                        {
                            positions[typeIndex].RemoveAt(index);
                            tiles[typeIndex].RemoveAt(index);
                            removed = true;
                        }
                        else
                        {
                            positions[typeIndex].Add(pos);
                            tiles[typeIndex].Add(_currentTile[typeIndex]);
                        }
                    }

                    if (selected && !removed)
                    {
                        var tile = Resources.Load<Tile>(tiles[typeIndex][index]);
                        var texture = AssetPreview.GetAssetPreview(tile.sprite);

                        if (texture)
                        {
                            texture.filterMode = FilterMode.Point;
                            GUI.DrawTexture(rect, texture);
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

                if (string.IsNullOrEmpty(path))
                    return;

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

                for (int i = 0; i < cellSizes.Length; i++)
                {
                    cellSizes[i] = -1;
                }

                _fileName = "";

                for (int i = 0; i < _types.Length; i++)
                {
                    ShowGrid((PoolSystem.TileType) _types.GetValue(i));
                }
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
                if (cellSizes[i] <= 0)
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