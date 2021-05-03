using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Value List of type `GamePhase`. Inherits from `AtomValueList&lt;GamePhase, GamePhaseEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/GamePhase", fileName = "GamePhaseValueList")]
    public sealed class GamePhaseValueList : AtomValueList<GamePhase, GamePhaseEvent> { }
}
