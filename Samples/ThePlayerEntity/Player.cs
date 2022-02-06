namespace MiddleMast.GameplayFramework.Samples.ThePlayerEntity
{
    public class Player : Entity
    {
        private HealthComponent _health;
        private MovementComponent _movement;

        public override void Setup()
        {
            base.Setup();

            _health.OnDamageTaken += HandleDamageTaken;
        }

        private void HandleDamageTaken()
        {
            if (_health.HealthRatio <= .5f)
            {
                _movement.Speed *= .66f;
            }
        }
    }
}
