namespace MiddleMast.GameplayFramework.Tests
{
    public class HumbleSystem : System
    {
        public bool IsSetup { get; private set; } = false;

        public override void Setup()
        {
            base.Setup();

            IsSetup = true;
        }
    }
}
