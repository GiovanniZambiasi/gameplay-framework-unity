namespace MiddleMast.GameplayFramework.Tests
{
    public class DependantSystem : System
    {
        private IHumbleDependency _dependency;

        public bool HasFulfilledDependency => _dependency != null;

        public void TryFindDependencyInScene()
        {
            _dependency = this.FindDependencyInScene<IHumbleDependency>();
        }

        public void TryFindDependency()
        {
            _dependency = this.FindDependency<IHumbleDependency>();
        }
    }
}
