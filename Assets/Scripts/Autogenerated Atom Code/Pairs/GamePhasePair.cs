using System;
using UnityEngine;
namespace UnityAtoms
{
    /// <summary>
    /// IPair of type `&lt;GamePhase&gt;`. Inherits from `IPair&lt;GamePhase&gt;`.
    /// </summary>
    [Serializable]
    public struct GamePhasePair : IPair<GamePhase>
    {
        public GamePhase Item1 { get => _item1; set => _item1 = value; }
        public GamePhase Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private GamePhase _item1;
        [SerializeField]
        private GamePhase _item2;

        public void Deconstruct(out GamePhase item1, out GamePhase item2) { item1 = Item1; item2 = Item2; }
    }
}