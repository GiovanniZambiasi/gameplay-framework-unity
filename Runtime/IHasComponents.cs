﻿using UnityEngine;

namespace MiddleMast.GameplayFramework
{
    /// <summary>
    /// Useful interface to access <see cref="MonoBehaviour"/>'s GetComponent methods
    /// </summary>
    public interface IHasComponents
    {
        TComponent GetComponent<TComponent>();

        bool TryGetComponent<TComponent>(out TComponent component);

        TComponent[] GetComponents<TComponent>();

        TComponent GetComponentInParent<TComponent>();

        TComponent[] GetComponentsInParent<TComponent>();

        TComponent GetComponentInChildren<TComponent>();

        TComponent[] GetComponentsInChildren<TComponent>();
    }
}
