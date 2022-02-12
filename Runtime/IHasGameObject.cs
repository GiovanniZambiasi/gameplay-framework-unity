using UnityEngine;

namespace MiddleMast.GameplayFramework
{
    /// <summary>
    /// Useful interface to access a <see cref="MonoBehaviour"/>'s gameObject property without needing a reference to a concrete type
    /// </summary>
    public interface IHasGameObject
    {
        GameObject gameObject { get; }  // Lowercase 'g' so it gets implemented automatically in MonoBehaviours
    }
}
