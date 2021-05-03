using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Instancer of type `GamePhase`. Inherits from `AtomEventInstancer&lt;GamePhase, GamePhaseEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/GamePhase Event Instancer")]
    public class GamePhaseEventInstancer : AtomEventInstancer<GamePhase, GamePhaseEvent> { }
}
