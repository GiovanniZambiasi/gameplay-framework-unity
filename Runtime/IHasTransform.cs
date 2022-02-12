using UnityEngine;

namespace MiddleMast.GameplayFramework
{
    /// <summary>
    /// Useful interface to access a <see cref="MonoBehaviour"/>'s transform property without needing a reference to a concrete type
    /// </summary>
    public interface IHasTransform
    {
        Transform transform { get; }    // Lowercase 't' so it gets implemented automatically in MonoBehaviours
    }
}
