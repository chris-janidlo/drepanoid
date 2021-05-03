using System;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference of type `GamePhasePair`. Inherits from `AtomEventReference&lt;GamePhasePair, GamePhaseVariable, GamePhasePairEvent, GamePhaseVariableInstancer, GamePhasePairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class GamePhasePairEventReference : AtomEventReference<
        GamePhasePair,
        GamePhaseVariable,
        GamePhasePairEvent,
        GamePhaseVariableInstancer,
        GamePhasePairEventInstancer>, IGetEvent 
    { }
}
