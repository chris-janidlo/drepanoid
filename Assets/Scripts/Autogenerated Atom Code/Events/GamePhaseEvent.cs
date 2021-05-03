using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event of type `GamePhase`. Inherits from `AtomEvent&lt;GamePhase&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/GamePhase", fileName = "GamePhaseEvent")]
    public sealed class GamePhaseEvent : AtomEvent<GamePhase> { }
}
