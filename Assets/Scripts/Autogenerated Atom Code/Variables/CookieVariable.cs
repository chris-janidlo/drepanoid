using Drepanoid;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Variable of type `Cookie`. Inherits from `EquatableAtomVariable&lt;Cookie, CookiePair, CookieEvent, CookiePairEvent, CookieCookieFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/Cookie", fileName = "CookieVariable")]
    public sealed class CookieVariable : EquatableAtomVariable<Cookie, CookiePair, CookieEvent, CookiePairEvent, CookieCookieFunction> { }
}
