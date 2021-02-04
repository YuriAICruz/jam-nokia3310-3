using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace DefaultNamespace.UI
{
    [RequireComponent(typeof(Button))]
    public class Navigation : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [HideInInspector]
        public Button Selection;
        public Navigation Right;
        public Navigation Left;
        private bool _selected;
        public bool IsSelected => _selected; 

        private void Awake()
        {
            Selection = GetComponent<Button>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            _selected = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _selected = false;
        }
    }
}