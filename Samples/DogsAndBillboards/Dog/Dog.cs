namespace MiddleMast.GameplayFramework.DogsAndBillboards.Dog
{
    public class Dog : Entity
    {
        private DogAnimations _animations;

        public void Setup(BreedData breed)
        {
            _animations = GetComponent<DogAnimations>();
            _animations.Setup(breed);
        }
    }
}
