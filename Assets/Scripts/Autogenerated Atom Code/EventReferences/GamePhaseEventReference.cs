using System;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference of type `GamePhase`. Inherits from `AtomEventReference&lt;GamePhase, GamePhaseVariable, GamePhaseEvent, GamePhaseVariableInstancer, GamePhaseEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class GamePhaseEventReference : AtomEventReference<
        GamePhase,
        GamePhaseVariable,
        GamePhaseEvent,
        GamePhaseVariableInstancer,
        GamePhaseEventInstancer>, IGetEvent 
    { }
}
