using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.SystemsAndManagers
{
    public class SampleSystem : System
    {
        public override void Setup()
        {
            Debug.Log("System begun setup!");

            base.Setup();
        }
    }
}
