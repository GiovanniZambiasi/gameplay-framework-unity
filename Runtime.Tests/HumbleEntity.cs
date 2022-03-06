using System;

namespace MiddleMast.GameplayFramework.Tests
{
    public class HumbleEntity : Entity
    {
        public event Action OnDisposed;

        public bool TickedAtLeastOnce { get; private set; } = false;

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            TickedAtLeastOnce = true;
        }

        public override void Dispose()
        {
            base.Dispose();
            OnDisposed?.Invoke();
        }
    }
}
