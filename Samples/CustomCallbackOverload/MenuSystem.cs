using MiddleMast.GameplayFramework.Samples.CustomCallbackOverload.Store;
using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.CustomCallbackOverload
{
    public class MenuSystem : System
    {
        [SerializeField] private StoreData _storeData;

        protected override void SetupManagers()
        {
            base.SetupManagers();

            StoreManager storeManager = GetManager<StoreManager>();
            storeManager.Setup(_storeData);
        }
    }
}
