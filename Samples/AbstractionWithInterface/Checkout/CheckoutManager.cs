using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.AbstractionWithInterface.Clients
{
    public class CheckoutManager : MonoBehaviour
    {
        private ICheckOutHandler _checkOutHandler;

        public void Setup(ICheckOutHandler checkoutHandler)
        {
            _checkOutHandler = checkoutHandler;         // Dependency gets fulfilled
        }

        private void HandleCheckoutFinished(GameObject client)
        {
            _checkOutHandler.HandleCheckOut(client);    // Communicates with ClientManager with abstraction
        }
    }
}
