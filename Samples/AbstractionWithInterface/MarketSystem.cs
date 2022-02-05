using MiddleMast.GameplayFramework.Samples.AbstractionWithInterface.Clients;

namespace MiddleMast.GameplayFramework.Samples.AbstractionWithInterface
{
    public class MarketSystem : System
    {
        protected override void SetupManagers()
        {
            base.SetupManagers();

            ClientManager clientManager = GetManager<ClientManager>();

            CheckoutManager checkoutManager = GetManager<CheckoutManager>();
            checkoutManager.Setup(clientManager);
        }
    }
}
