using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.WizardsAndGoblins
{
    public interface ISpellFactory
    {
        ISpell CreateSpell(GameObject spellPrefab, Vector3 position, Vector3 direction);
    }
}
