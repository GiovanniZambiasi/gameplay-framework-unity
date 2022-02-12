using System;

namespace MiddleMast.GameplayFramework.Tests
{
    public class HumbleManager : Manager
    {
        public event Action<HumbleManager> OnSetup;
        public event Action<HumbleManager> OnDispose;
        public event Action<HumbleManager> OnTick;

        public override void Setup()
        {
            base.Setup();

            OnSetup?.Invoke(this);
        }

        public override void Dispose()
        {
            base.Dispose();

            OnDispose?.Invoke(this);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            OnTick?.Invoke(this);
        }
    }
}
