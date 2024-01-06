using MiddleMast.GameplayFramework;
using UnityEngine;

namespace WizardsAndGoblins.Wizards
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
