using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms
{
    /// <summary>
    /// Set variable value Action of type `Cookie`. Inherits from `SetVariableValue&lt;Cookie, CookiePair, CookieVariable, CookieConstant, CookieReference, CookieEvent, CookiePairEvent, CookieVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/Cookie", fileName = "SetCookieVariableValue")]
    public sealed class SetCookieVariableValue : SetVariableValue<
        Cookie,
        CookiePair,
        CookieVariable,
        CookieConstant,
        CookieReference,
        CookieEvent,
        CookiePairEvent,
        CookieCookieFunction,
        CookieVariableInstancer>
    { }
}
