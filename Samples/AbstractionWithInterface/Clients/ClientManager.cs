using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.AbstractionWithInterface
{
    public class ClientManager : MonoBehaviour, ICheckOutHandler
    {
        public void HandleCheckOut(GameObject client)
        {
            // Sends client away
        }
    }
}
