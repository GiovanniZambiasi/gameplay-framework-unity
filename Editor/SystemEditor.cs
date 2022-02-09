using UnityEditor;
using UnityEngine;

namespace MiddleMast.GameplayFramework.Editor
{
    [CustomEditor(typeof(System), true)]
    public class SystemEditor : UnityEditor.Editor
    {
        private System _system;
        private SerializedProperty _managers;
        private SerializedProperty _setupTiming;

        private void Awake()
        {
            Setup();
        }

        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                RefreshManagers();
            }

            DrawDefaultInspector();
        }

        private void Setup()
        {
            _system = target as System;
            _managers = serializedObject.FindProperty(nameof(_managers));
            _setupTiming = serializedObject.FindProperty(nameof(_setupTiming));
        }

        private void OnValidate()
        {
            RefreshManagers();
        }

        private void RefreshManagers()
        {
            Transform systemTransform = _system.transform;

            _managers.ClearArray();

            for (int i = 0; i < systemTransform.childCount; i++)
            {
                Transform child = systemTransform.GetChild(i);

                if (child.TryGetComponent(out Manager manager))
                {
                    _managers.arraySize++;
                    SerializedProperty arrayElement = _managers.GetArrayElementAtIndex(i);
                    arrayElement.objectReferenceValue = manager;
                }
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
