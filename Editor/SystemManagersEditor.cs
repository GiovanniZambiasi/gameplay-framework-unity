using MiddleMast.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MiddleMast.GameplayFramework.Editor
{
    public class SystemManagersEditor
    {
        private readonly List<GameObject> _gameObjectList = new List<GameObject>();
        private readonly List<Manager> _managerList = new List<Manager>();
        private System _system;
        private SerializedObject _systemSerialized;
        private SerializedProperty _managers;

        public SystemManagersEditor(SerializedObject system, SerializedProperty managersProperty)
        {
            _systemSerialized = system;
            _system = _systemSerialized.targetObject as System;
            _managers = managersProperty;
        }

        public void Draw()
        {
            _gameObjectList.Clear();
            FindUnregisteredManagers(_gameObjectList);

            if (_gameObjectList.Count > 0)
            {
                DrawUnregisteredManagers(_gameObjectList);
            }
            else if(!ManagerOrderMatchesHierarchy())
            {
                DrawOrderWarning();
            }

            if (GUILayout.Button(nameof(RefreshManagers)))
            {
                RefreshManagers();
            }
        }

        public bool ManagerOrderMatchesHierarchy()
        {
            _managerList.Clear();
            _system.GetFirstLevelChildComponents(_managerList);

            if (_managerList.Count != _managers.arraySize)
            {
                return false;
            }

            for (int i = 0; i < _managers.arraySize; i++)
            {
                SerializedProperty managerProperty = _managers.GetArrayElementAtIndex(i);
                Manager manager = managerProperty.objectReferenceValue as Manager;

                Manager managerInHierarchy = _managerList[i];

                if (manager != managerInHierarchy)
                {
                    return false;
                }
            }

            return true;
        }

        public void RefreshManagers()
        {
            _systemSerialized.Update();

            _managers.ClearArray();
            _managerList.Clear();
            _system.GetFirstLevelChildComponents(_managerList);

            for (int i = 0; i < _managerList.Count; i++)
            {
                Manager manager = _managerList[i];
                _managers.arraySize++;
                SerializedProperty arrayElement = _managers.GetArrayElementAtIndex(i);
                arrayElement.objectReferenceValue = manager;
            }

            _systemSerialized.ApplyModifiedProperties();
        }

        private void FindUnregisteredManagers(List<GameObject> unregisteredManagers)
        {
            _managerList.Clear();
            _system.GetFirstLevelChildComponents(_managerList);

            for (int i = _managerList.Count - 1; i >= 0; i--)
            {
                Manager manager = _managerList[i];

                if (!_managers.ContainsArrayElement(manager))
                {
                    unregisteredManagers.Add(manager.gameObject);
                }
            }
        }

        private void DrawUnregisteredManagers(List<GameObject> unregisteredManagers)
        {
            string message = $"{unregisteredManagers.Count.ToString()} unregistered {nameof(Manager)}s found:\n";

            for (int i = 0; i < unregisteredManagers.Count; i++)
            {
                GameObject manager = unregisteredManagers[i];
                message += $"  '{manager.name}';\n";
            }

            message += $"\nUse {nameof(RefreshManagers)} to fix";

            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }

        private void DrawOrderWarning()
        {
            EditorGUILayout.HelpBox($"Registered {nameof(Manager)}s don't match {_system.gameObject.name}'s hierarchy. Use {nameof(RefreshManagers)} to fix", MessageType.Warning);
        }
    }
}
