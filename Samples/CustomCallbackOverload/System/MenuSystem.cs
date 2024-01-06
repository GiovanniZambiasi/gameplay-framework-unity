using CustomCallbackOverload.Store.Manager;
using UnityEngine;

namespace CustomCallbackOverload.System
{
    public class MenuSystem : MiddleMast.GameplayFramework.System
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
