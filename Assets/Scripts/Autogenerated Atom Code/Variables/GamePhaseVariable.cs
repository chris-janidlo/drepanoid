using UnityEngine;
using System;

namespace UnityAtoms
{
    /// <summary>
    /// Variable of type `GamePhase`. Inherits from `AtomVariable&lt;GamePhase, GamePhasePair, GamePhaseEvent, GamePhasePairEvent, GamePhaseGamePhaseFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/GamePhase", fileName = "GamePhaseVariable")]
    public sealed class GamePhaseVariable : AtomVariable<GamePhase, GamePhasePair, GamePhaseEvent, GamePhasePairEvent, GamePhaseGamePhaseFunction>
    {
        protected override bool ValueEquals(GamePhase other)
        {
            return Value == other;
        }
    }
}
