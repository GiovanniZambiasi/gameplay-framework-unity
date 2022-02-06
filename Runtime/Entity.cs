using UnityEngine;

namespace MiddleMast.GameplayFramework
{
    [DisallowMultipleComponent]
    public abstract class Entity : MonoBehaviour
    {
        public virtual void Setup() { }

        public virtual void Dispose() { }

        public virtual void Tick(float deltaTime) { }
    }
}
