using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms
{
    /// <summary>
    /// Set variable value Action of type `GamePhase`. Inherits from `SetVariableValue&lt;GamePhase, GamePhasePair, GamePhaseVariable, GamePhaseConstant, GamePhaseReference, GamePhaseEvent, GamePhasePairEvent, GamePhaseVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/GamePhase", fileName = "SetGamePhaseVariableValue")]
    public sealed class SetGamePhaseVariableValue : SetVariableValue<
        GamePhase,
        GamePhasePair,
        GamePhaseVariable,
        GamePhaseConstant,
        GamePhaseReference,
        GamePhaseEvent,
        GamePhasePairEvent,
        GamePhaseGamePhaseFunction,
        GamePhaseVariableInstancer>
    { }
}
