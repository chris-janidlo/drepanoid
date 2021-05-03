using System;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms
{
    /// <summary>
    /// Reference of type `GamePhase`. Inherits from `AtomReference&lt;GamePhase, GamePhasePair, GamePhaseConstant, GamePhaseVariable, GamePhaseEvent, GamePhasePairEvent, GamePhaseGamePhaseFunction, GamePhaseVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class GamePhaseReference : AtomReference<
        GamePhase,
        GamePhasePair,
        GamePhaseConstant,
        GamePhaseVariable,
        GamePhaseEvent,
        GamePhasePairEvent,
        GamePhaseGamePhaseFunction,
        GamePhaseVariableInstancer>, IEquatable<GamePhaseReference>
    {
        public GamePhaseReference() : base() { }
        public GamePhaseReference(GamePhase value) : base(value) { }
        public bool Equals(GamePhaseReference other) { return base.Equals(other); }
        protected override bool ValueEquals(GamePhase other)
        {
            throw new NotImplementedException();
        } 
    }
}
