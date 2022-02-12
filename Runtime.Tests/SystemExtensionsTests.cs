using NUnit.Framework;
using System;
using UnityEngine.SceneManagement;

namespace MiddleMast.GameplayFramework.Tests
{
    public class SystemExtensionsTests
    {
        [Test]
        public void FindsDependencyInSameScene()
        {
            DependantSystem dependant = SystemTestUtilities.CreateSystem<DependantSystem>(Array.Empty<Type>());

            SystemTestUtilities.CreateSystem<DependeeSystem>(Array.Empty<Type>());

            dependant.TryFindDependencyInScene();

            Assert.IsTrue(dependant.HasFulfilledDependency);
        }

        [Test]
        public void FindDependencyInSceneDoesntSearchOtherScenes()
        {
            DependantSystem dependant = SystemTestUtilities.CreateSystem<DependantSystem>(Array.Empty<Type>());
            DependeeSystem dependee = SystemTestUtilities.CreateSystem<DependeeSystem>(Array.Empty<Type>());

            Scene scene = SceneManager.CreateScene($"{nameof(FindDependencyInSceneDoesntSearchOtherScenes)}'s scene");
            SceneManager.MoveGameObjectToScene(dependee.gameObject, scene);

            dependant.TryFindDependencyInScene();

            Assert.IsFalse(dependant.HasFulfilledDependency);

            SceneManager.UnloadSceneAsync(scene);
        }

        [Test]
        public void FindsDependencyInOtherScene()
        {
            DependantSystem dependant = SystemTestUtilities.CreateSystem<DependantSystem>(Array.Empty<Type>());
            DependeeSystem dependee = SystemTestUtilities.CreateSystem<DependeeSystem>(Array.Empty<Type>());

            Scene scene = SceneManager.CreateScene($"{nameof(FindsDependencyInOtherScene)}'s Scene");
            SceneManager.MoveGameObjectToScene(dependee.gameObject, scene);

            dependant.TryFindDependency();

            Assert.IsTrue(dependant.HasFulfilledDependency);

            SceneManager.UnloadSceneAsync(scene);
        }
    }
}
