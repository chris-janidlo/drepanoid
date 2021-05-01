using System;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms
{
    /// <summary>
    /// Reference of type `Cookie`. Inherits from `EquatableAtomReference&lt;Cookie, CookiePair, CookieConstant, CookieVariable, CookieEvent, CookiePairEvent, CookieCookieFunction, CookieVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class CookieReference : EquatableAtomReference<
        Cookie,
        CookiePair,
        CookieConstant,
        CookieVariable,
        CookieEvent,
        CookiePairEvent,
        CookieCookieFunction,
        CookieVariableInstancer>, IEquatable<CookieReference>
    {
        public CookieReference() : base() { }
        public CookieReference(Cookie value) : base(value) { }
        public bool Equals(CookieReference other) { return base.Equals(other); }
    }
}
