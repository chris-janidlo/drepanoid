using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference Listener of type `CookiePair`. Inherits from `AtomEventReferenceListener&lt;CookiePair, CookiePairEvent, CookiePairEventReference, CookiePairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/CookiePair Event Reference Listener")]
    public sealed class CookiePairEventReferenceListener : AtomEventReferenceListener<
        CookiePair,
        CookiePairEvent,
        CookiePairEventReference,
        CookiePairUnityEvent>
    { }
}
