using System;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference of type `CookiePair`. Inherits from `AtomEventReference&lt;CookiePair, CookieVariable, CookiePairEvent, CookieVariableInstancer, CookiePairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class CookiePairEventReference : AtomEventReference<
        CookiePair,
        CookieVariable,
        CookiePairEvent,
        CookieVariableInstancer,
        CookiePairEventInstancer>, IGetEvent 
    { }
}
