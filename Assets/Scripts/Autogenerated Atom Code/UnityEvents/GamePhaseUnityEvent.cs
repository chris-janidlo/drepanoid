using System;
using UnityEngine.Events;

namespace UnityAtoms
{
    /// <summary>
    /// None generic Unity Event of type `GamePhase`. Inherits from `UnityEvent&lt;GamePhase&gt;`.
    /// </summary>
    [Serializable]
    public sealed class GamePhaseUnityEvent : UnityEvent<GamePhase> { }
}
