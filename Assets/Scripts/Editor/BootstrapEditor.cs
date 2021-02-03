using DefaultNamespace;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [CustomEditor(typeof(Bootstrap))]
    public class BootstrapEditor : UnityEditor.Editor
    {
        private Bootstrap _self;

        private void Awake()
        {
            _self = target as Bootstrap;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Load Blocks"))
            {
                var resources = Resources.LoadAll<TextAsset>("Blocks");

                _self.blocks = new PoolSystem.Block[resources.Length];

                for (int i = 0; i < _self.blocks.Length; i++)
                {
                    _self.blocks[i] = JsonConvert.DeserializeObject<PoolSystem.Block>(resources[i].text);
                }

                EditorUtility.SetDirty(target);
            }
        }
    }
}