using System.Collections.Generic;
using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.WizardsAndGoblins.Spells
{
    internal class SpellManager : Manager, ISpellFactory
    {
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

        public ISpell CreateSpell(GameObject spellPrefab, Vector3 position, Vector3 direction)
        {
            if (!spellPrefab.TryGetComponent(out ISpell spell))
            {
                Debug.LogError($"Prefab '{spellPrefab.name}' is not a spell!");
                return null;
            }

            spell = Instantiate(spellPrefab, position, Quaternion.LookRotation(direction)).GetComponent<ISpell>();

            if (spell is Entity entity)
            {
                _spells.Add(entity);
            }

            return spell;
        }
    }
}
