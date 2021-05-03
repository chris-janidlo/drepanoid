using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms
{
    /// <summary>
    /// Variable Instancer of type `GamePhase`. Inherits from `AtomVariableInstancer&lt;GamePhaseVariable, GamePhasePair, GamePhase, GamePhaseEvent, GamePhasePairEvent, GamePhaseGamePhaseFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/GamePhase Variable Instancer")]
    public class GamePhaseVariableInstancer : AtomVariableInstancer<
        GamePhaseVariable,
        GamePhasePair,
        GamePhase,
        GamePhaseEvent,
        GamePhasePairEvent,
        GamePhaseGamePhaseFunction>
    { }
}
