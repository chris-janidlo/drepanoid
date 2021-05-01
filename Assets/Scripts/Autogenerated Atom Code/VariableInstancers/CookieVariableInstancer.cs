using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms
{
    /// <summary>
    /// Variable Instancer of type `Cookie`. Inherits from `AtomVariableInstancer&lt;CookieVariable, CookiePair, Cookie, CookieEvent, CookiePairEvent, CookieCookieFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/Cookie Variable Instancer")]
    public class CookieVariableInstancer : AtomVariableInstancer<
        CookieVariable,
        CookiePair,
        Cookie,
        CookieEvent,
        CookiePairEvent,
        CookieCookieFunction>
    { }
}
