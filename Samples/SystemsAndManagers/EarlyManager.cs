using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.SystemsAndManagers
{
    public class EarlyManager : Manager
    {
        public override void Setup()
        {
            base.Setup();

            Debug.Log("Early manager setting up!");
        }

        public override void Dispose()
        {
            base.Dispose();

            Debug.Log("Early manager disposed!");
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            Debug.Log("Early manager tick!");
        }
    }
}
