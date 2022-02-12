using UnityEditor;

namespace MiddleMast.GameplayFramework.Editor
{
    [CustomEditor(typeof(System), true)]
    public class SystemEditor : UnityEditor.Editor
    {
        private System _system;
        private SerializedProperty _setupTiming;
        private SystemManagersEditor _managersEditor;

        public SystemManagersEditor ManagersEditor => _managersEditor;

        private bool IsSetup => _system != null && _setupTiming != null;

        private void Awake()
        {
            Setup();
        }

        public override void OnInspectorGUI()
        {
            if (!IsSetup)
            {
                Setup();
            }

            DrawDefaultInspector();

            _managersEditor.Draw();
        }

        private void Setup()
        {
            _system = target as System;

            SerializedProperty managers = serializedObject.FindProperty("_managers");
            _managersEditor = new SystemManagersEditor(serializedObject, managers);

            _setupTiming = serializedObject.FindProperty(nameof(_setupTiming));
        }
    }
}
