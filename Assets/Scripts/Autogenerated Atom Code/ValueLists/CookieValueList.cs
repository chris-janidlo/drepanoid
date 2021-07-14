using Drepanoid;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Value List of type `Cookie`. Inherits from `AtomValueList&lt;Cookie, CookieEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/Cookie", fileName = "CookieValueList")]
    public sealed class CookieValueList : AtomValueList<Cookie, CookieEvent> { }
}
