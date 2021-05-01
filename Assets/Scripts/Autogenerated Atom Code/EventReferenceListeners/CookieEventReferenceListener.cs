using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference Listener of type `Cookie`. Inherits from `AtomEventReferenceListener&lt;Cookie, CookieEvent, CookieEventReference, CookieUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/Cookie Event Reference Listener")]
    public sealed class CookieEventReferenceListener : AtomEventReferenceListener<
        Cookie,
        CookieEvent,
        CookieEventReference,
        CookieUnityEvent>
    { }
}
