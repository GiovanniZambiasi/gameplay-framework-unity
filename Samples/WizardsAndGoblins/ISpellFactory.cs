using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.WizardsAndGoblins
{
    public interface ISpellFactory
    {
        ISpell CreateSpell(Vector3 position, Vector3 direction);
    }
}
