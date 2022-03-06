using UnityEngine.SceneManagement;

namespace MiddleMast.GameplayFramework
{
    public static class SystemExtensions
    {
        /// <summary>
        /// Goes through all root objects in a scene. Useful when a system is trying to fulfill a dependency with another system's interface.<br></br>
        /// <b>You should never try to find another system by type.</b> This method should only get called with interfaces as the generic parameter
        /// </summary>
        public static TDependency FindDependencyInScene<TDependency>(this System system, bool includeInactive = true)
        {
            return system.gameObject.scene.FindRootObjectOfType<TDependency>(includeInactive);
        }

        /// <summary>
        /// Goes through all active scenes, trying to find a root object which implements <see cref="TDependency"/>.
        /// Useful when a system is trying to fulfill a dependency with another system's interface.<br></br>
        /// <b>You should never try to find another system by type.</b> This method should only get called with interfaces as the generic parameter
        /// </summary>
        public static TDependency FindDependency<TDependency>(this System system, bool includeInactive = true)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                TDependency dependency = scene.FindRootObjectOfType<TDependency>(includeInactive);

                if (dependency != null)
                {
                    return dependency;
                }
            }

            return default;
        }
    }
}
