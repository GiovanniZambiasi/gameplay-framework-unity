using MiddleMast.GameplayFramework;

namespace WizardsAndGoblins.Spells
{
    internal class Heal : Entity, ISpell
    {
        public void Activate()
        {
            // Finds nearby healable objects using collision, and add some value to their health
        }
    }
}
