using MiddleMast.GameplayFramework.Tests;
using System.Collections;
using NUnit.Framework;
using System;
using UnityEngine.TestTools;

namespace MiddleMast.GameplayFramework.Editor
{
    public class SystemEditorTests
    {
        [Test]
        public void RecognizesPerfectHierarchy()
        {
            Type[] managers = new[]
            {
                typeof(HumbleManager),
                typeof(HumbleManager),
            };

            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(managers);

            SystemEditor editor = CreateEditor(system);
            SystemManagersEditor managersEditor = editor.ManagersEditor;

            Assert.IsTrue(managersEditor.ManagerOrderMatchesHierarchy());
        }

        [Test]
        public void FindsUnregisteredManagerMismatch()
        {
            Type[] managers = new[]
            {
                typeof(HumbleManager),
                typeof(HumbleManager),
            };

            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(managers);
            Manager extraManager = SystemTestUtilities.CreateManager(system, typeof(HumbleManager));

            SystemEditor editor = CreateEditor(system);
            SystemManagersEditor managersEditor = editor.ManagersEditor;

            Assert.IsFalse(managersEditor.ManagerOrderMatchesHierarchy());
        }

        [Test]
        public void FindsManagersOutOfOrderMismatch()
        {
            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>());

            Manager firstManager = SystemTestUtilities.CreateManager(system, typeof(HumbleManager));
            Manager secondManager = SystemTestUtilities.CreateManager(system, typeof(HumbleManager));

            system.RegisterManager(secondManager);
            system.RegisterManager(firstManager);

            SystemEditor editor = CreateEditor(system);
            SystemManagersEditor managersEditor = editor.ManagersEditor;

            Assert.IsFalse(managersEditor.ManagerOrderMatchesHierarchy());
        }

        [Test]
        public void FindsDuplicateManagersMismatch()
        {
            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>());

            Manager firstManager = SystemTestUtilities.CreateManager(system, typeof(HumbleManager));

            system.RegisterManager(firstManager);
            system.RegisterManager(firstManager);

            SystemEditor editor = CreateEditor(system);
            SystemManagersEditor managersEditor = editor.ManagersEditor;

            Assert.IsFalse(managersEditor.ManagerOrderMatchesHierarchy());
        }

        [Test]
        public void RefreshManagersCreatesPerfectHierarchy()
        {
            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>());

            SystemTestUtilities.CreateManager(system, typeof(HumbleManager));
            SystemTestUtilities.CreateManager(system, typeof(HumbleManager));

            SystemEditor editor = CreateEditor(system);
            SystemManagersEditor managersEditor = editor.ManagersEditor;
            managersEditor.RefreshManagers();

            Assert.IsTrue(managersEditor.ManagerOrderMatchesHierarchy());
        }

        private SystemEditor CreateEditor(System system)
        {
            return UnityEditor.Editor.CreateEditor(system, typeof(SystemEditor)) as SystemEditor;
        }
    }
}

