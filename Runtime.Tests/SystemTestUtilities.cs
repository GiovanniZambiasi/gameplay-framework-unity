using System;
using UnityEngine;

namespace MiddleMast.GameplayFramework.Tests
{
    public static class SystemTestUtilities
    {
        private static GameObject _systemModelObject;

        private static GameObject SystemModelObject
        {
            get
            {
                if (_systemModelObject == null)
                {
                    _systemModelObject = new GameObject("ModelObject");
                    _systemModelObject.SetActive(false);
                }

                return _systemModelObject;
            }
        }

        public static TSystem CreateSystem<TSystem>(Type[] managers, System.SetupTimings setupTiming = System.SetupTimings.Custom) where TSystem : System
        {
            GameObject systemGameObject = UnityEngine.Object.Instantiate(SystemModelObject);
            systemGameObject.name = nameof(TSystem);

            TSystem system = systemGameObject.AddComponent<TSystem>();
            system.SetupTiming = setupTiming;

            CreateAndRegisterManagers(system, managers);

            systemGameObject.SetActive(true);

            return system;
        }

        public static TManager CreateManager<TManager>(System system) where TManager : Manager
        {
            return CreateManager(system, typeof(TManager)) as TManager;
        }

        public static Manager CreateManager(System system, Type type)
        {
            GameObject managerGameObject = new GameObject(type.Name, type);
            managerGameObject.transform.SetParent(system.transform);

            Manager manager = managerGameObject.GetComponent<Manager>();

            return manager;
        }

        private static void CreateAndRegisterManagers(System system, Type[] managers)
        {
            foreach (Type managerType in managers)
            {
                Manager manager = CreateManager(system, managerType);
                system.RegisterManager(manager);
            }
        }
    }
}
