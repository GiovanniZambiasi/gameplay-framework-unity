using System;
using UnityEngine;

namespace MiddleMast.GameplayFramework.Tests
{
    public static class SystemTestUtilities
    {
        public static TSystem CreateSystem<TSystem>(Type[] managers) where TSystem : System
        {
            GameObject systemGameObject = new GameObject($"{nameof(TSystem)}", typeof(TSystem));
            TSystem system = systemGameObject.GetComponent<TSystem>();

            CreateAndRegisterManagers(system, managers);

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
