using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.WizardsAndGoblins.Wizards
{
    internal class Wizard : Entity
    {
        private GameObject _spellPrefab;
        private ISpellFactory _spellFactory;

        public void Setup(GameObject spellPrefab, ISpellFactory spellFactory)
        {
            _spellPrefab = spellPrefab;
            _spellFactory = spellFactory;
        }

        public void CastSpell(Vector3 direction)
        {
            ISpell spell = _spellFactory.CreateSpell(_spellPrefab, transform.position, direction);
            spell.Activate();
        }
    }
}
