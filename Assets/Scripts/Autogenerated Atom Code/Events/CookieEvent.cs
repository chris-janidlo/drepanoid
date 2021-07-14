using Drepanoid;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event of type `Cookie`. Inherits from `AtomEvent&lt;Cookie&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/Cookie", fileName = "CookieEvent")]
    public sealed class CookieEvent : AtomEvent<Cookie> { }
}
