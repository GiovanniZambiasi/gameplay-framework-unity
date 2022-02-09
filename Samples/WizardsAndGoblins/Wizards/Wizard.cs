using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.WizardsAndGoblins.Wizards
{
    internal class Wizard : Entity
    {
        private ISpellFactory _spellFactory;

        public void Setup(ISpellFactory spellFactory)
        {
            _spellFactory = spellFactory;
        }

        public void CastSpell(Vector3 direction)
        {
            ISpell spell = _spellFactory.CreateSpell(transform.position, direction);
            spell.Activate();
        }
    }
}
