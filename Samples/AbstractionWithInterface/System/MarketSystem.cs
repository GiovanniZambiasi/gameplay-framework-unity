using AbstractionWithInterface.Checkout.Manager;
using AbstractionWithInterface.Clients.Manager;

namespace AbstractionWithInterface.System
{
    public class MarketSystem : MiddleMast.GameplayFramework.System
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
