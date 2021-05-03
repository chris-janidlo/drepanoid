using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference Listener of type `GamePhasePair`. Inherits from `AtomEventReferenceListener&lt;GamePhasePair, GamePhasePairEvent, GamePhasePairEventReference, GamePhasePairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/GamePhasePair Event Reference Listener")]
    public sealed class GamePhasePairEventReferenceListener : AtomEventReferenceListener<
        GamePhasePair,
        GamePhasePairEvent,
        GamePhasePairEventReference,
        GamePhasePairUnityEvent>
    { }
}
