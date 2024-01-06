using MiddleMast.GameplayFramework;
using UnityEngine;

namespace WizardsAndGoblins.Spells
{
    internal class Fireball : Entity, ISpell
    {
        private Rigidbody _rigidbody;

        public override void Setup()
        {
            base.Setup();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Activate()
        {
            _rigidbody.AddRelativeForce(transform.forward * 10f, ForceMode.Impulse);
        }
    }
}
