using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.SystemsAndManagers
{
    public class MiddleManager : Manager
    {
        public override void Setup()
        {
            base.Setup();

            Debug.Log("Middle manager setting up!");
        }

        public override void Dispose()
        {
            base.Dispose();

            Debug.Log("Middle manager disposed!");
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            Debug.Log("Middle manager tick!");
        }
    }
}
