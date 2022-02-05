using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.SystemsAndManagers
{
    public class LateManager : Manager
    {
        public override void Setup()
        {
            base.Setup();

            Debug.Log("Late manager setting up!");
        }

        public override void Dispose()
        {
            base.Dispose();

            Debug.Log("Late manager disposed!");
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            Debug.Log("Late manager tick!");
        }
    }
}
