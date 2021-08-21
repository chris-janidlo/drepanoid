using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event of type `CookiePair`. Inherits from `AtomEvent&lt;CookiePair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/CookiePair", fileName = "CookiePairEvent")]
    public sealed class CookiePairEvent : AtomEvent<CookiePair> { }
}
