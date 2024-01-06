using UnityEngine;

namespace WizardsAndGoblins
{
    public interface ISpellFactory
    {
        ISpell CreateSpell(Vector3 position, Vector3 direction);
    }
}
