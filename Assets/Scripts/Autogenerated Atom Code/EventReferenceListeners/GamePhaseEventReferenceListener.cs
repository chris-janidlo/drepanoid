using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference Listener of type `GamePhase`. Inherits from `AtomEventReferenceListener&lt;GamePhase, GamePhaseEvent, GamePhaseEventReference, GamePhaseUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/GamePhase Event Reference Listener")]
    public sealed class GamePhaseEventReferenceListener : AtomEventReferenceListener<
        GamePhase,
        GamePhaseEvent,
        GamePhaseEventReference,
        GamePhaseUnityEvent>
    { }
}
