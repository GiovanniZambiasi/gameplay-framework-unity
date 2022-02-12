using System;
using UnityEditor;
using UnityEngine;

namespace MiddleMast.GameplayFramework.Editor
{
    [CustomEditor(typeof(Manager), true)]
    public class ManagerEditor : UnityEditor.Editor
    {
        private Manager _manager;

        public override void OnInspectorGUI()
        {
            if (_manager == null)
            {
                Setup();
            }

            base.OnInspectorGUI();

            UpdateAssignment();
        }

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            _manager = target as Manager;
        }

        private void UpdateAssignment()
        {
             Transform parent = _manager.transform.parent;

            if (parent == null)
            {
                EditorGUILayout.HelpBox($"This manager is at the root of a scene. Consider making it a child of its respective {nameof(System)}", MessageType.Warning);

                return;
            }

            if (!parent.TryGetComponent(out System system))
            {
                EditorGUILayout.HelpBox($"This {nameof(Manager)} is not a child of a {nameof(System)}", MessageType.Warning);
            }
        }
    }
}
