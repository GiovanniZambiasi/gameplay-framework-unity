using UnityEngine;

namespace AbstractionWithInterface.Clients.Manager
{
    public class ClientManager : MonoBehaviour, ICheckOutHandler
    {
        public void HandleCheckOut(GameObject client)
        {
            // Sends client away
        }
    }
}
