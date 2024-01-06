using MiddleMast.GameplayFramework;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsAndGoblins.Spells.Manager
{
    internal class SpellManager : MiddleMast.GameplayFramework.Manager, ISpellFactory
    {
        [SerializeField] private GameObject _spellPrefab;

        private List<Entity> _spells = new List<Entity>();

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            for (int i = 0; i < _spells.Count; i++)
            {
                Entity spell = _spells[i];
                spell.Tick(deltaTime);
            }
        }

        public ISpell CreateSpell(Vector3 position, Vector3 direction)
        {
            if (!_spellPrefab.TryGetComponent(out ISpell spell))
            {
                Debug.LogError($"Prefab '{_spellPrefab.name}' is not a spell!");
                return null;
            }

            spell = Instantiate(_spellPrefab, position, Quaternion.LookRotation(direction)).GetComponent<ISpell>();

            if (spell is Entity entity)
            {
                _spells.Add(entity);
            }

            return spell;
        }
    }
}
