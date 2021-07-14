using Drepanoid;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Instancer of type `Cookie`. Inherits from `AtomEventInstancer&lt;Cookie, CookieEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/Cookie Event Instancer")]
    public class CookieEventInstancer : AtomEventInstancer<Cookie, CookieEvent> { }
}
