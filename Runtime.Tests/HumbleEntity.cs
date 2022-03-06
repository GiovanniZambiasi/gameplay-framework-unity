using System;

namespace MiddleMast.GameplayFramework.Tests
{
    public class HumbleEntity : Entity
    {
        public event Action OnDisposed;
        public event Action<HumbleEntity> OnDisposeWanted;

        public bool TickedAtLeastOnce { get; private set; } = false;
        public bool DisposeInNextTick { get; set; } = false;

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            TickedAtLeastOnce = true;

            if (DisposeInNextTick)
            {
                OnDisposeWanted?.Invoke(this);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            OnDisposed?.Invoke();
        }
    }
}
