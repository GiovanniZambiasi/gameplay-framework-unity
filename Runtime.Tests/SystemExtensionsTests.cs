using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace MiddleMast.GameplayFramework.Tests
{
    public class SystemExtensionsTests
    {
        [UnityTest]
        public IEnumerator FindsDependencyInSameScene()
        {
            DependantSystem dependant = SystemTestUtilities.CreateSystem<DependantSystem>(Array.Empty<Type>());
            DependeeSystem dependee = SystemTestUtilities.CreateSystem<DependeeSystem>(Array.Empty<Type>());

            Scene scene = SceneManager.CreateScene($"{nameof(FindDependencyInSceneDoesntSearchOtherScenes)}'s scene");
            SceneManager.MoveGameObjectToScene(dependee.gameObject, scene);
            SceneManager.MoveGameObjectToScene(dependant.gameObject, scene);

            dependant.TryFindDependencyInScene();

            Assert.IsTrue(dependant.HasFulfilledDependency);

            UnityEngine.Object.Destroy(dependant.gameObject);
            UnityEngine.Object.Destroy(dependee.gameObject);
            yield return SceneManager.UnloadSceneAsync(scene);
        }

        [UnityTest]
        public IEnumerator FindDependencyInSceneDoesntSearchOtherScenes()
        {
            DependantSystem dependant = SystemTestUtilities.CreateSystem<DependantSystem>(Array.Empty<Type>());
            DependeeSystem dependee = SystemTestUtilities.CreateSystem<DependeeSystem>(Array.Empty<Type>());

            Scene scene = SceneManager.CreateScene($"{nameof(FindDependencyInSceneDoesntSearchOtherScenes)}'s scene");
            SceneManager.MoveGameObjectToScene(dependee.gameObject, scene);

            dependant.TryFindDependencyInScene();

            Assert.IsFalse(dependant.HasFulfilledDependency);

            UnityEngine.Object.Destroy(dependant.gameObject);
            UnityEngine.Object.Destroy(dependee.gameObject);
            yield return SceneManager.UnloadSceneAsync(scene);
        }

        [UnityTest]
        public IEnumerator FindsDependencyInOtherScene()
        {
            DependantSystem dependant = SystemTestUtilities.CreateSystem<DependantSystem>(Array.Empty<Type>());
            DependeeSystem dependee = SystemTestUtilities.CreateSystem<DependeeSystem>(Array.Empty<Type>());

            Scene scene = SceneManager.CreateScene($"{nameof(FindsDependencyInOtherScene)}'s Scene");
            SceneManager.MoveGameObjectToScene(dependee.gameObject, scene);

            dependant.TryFindDependency();

            Assert.IsTrue(dependant.HasFulfilledDependency);

            UnityEngine.Object.Destroy(dependant.gameObject);
            UnityEngine.Object.Destroy(dependee.gameObject);
            yield return SceneManager.UnloadSceneAsync(scene);
        }

        [Test]
        public void ReturnsNullWhenDependencyNotFound()
        {
            DependantSystem dependant = SystemTestUtilities.CreateSystem<DependantSystem>(Array.Empty<Type>());
            IHumbleDependency dependency = dependant.FindDependency<IHumbleDependency>(false);

            Assert.IsNull(dependency);

            UnityEngine.Object.Destroy(dependant.gameObject);
        }
    }
}
