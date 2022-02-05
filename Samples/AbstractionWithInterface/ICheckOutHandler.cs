using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.AbstractionWithInterface
{
    public interface ICheckOutHandler
    {
        void HandleCheckOut(GameObject client);
    }
}
