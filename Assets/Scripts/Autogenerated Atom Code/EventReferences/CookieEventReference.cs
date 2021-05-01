using System;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference of type `Cookie`. Inherits from `AtomEventReference&lt;Cookie, CookieVariable, CookieEvent, CookieVariableInstancer, CookieEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class CookieEventReference : AtomEventReference<
        Cookie,
        CookieVariable,
        CookieEvent,
        CookieVariableInstancer,
        CookieEventInstancer>, IGetEvent 
    { }
}
