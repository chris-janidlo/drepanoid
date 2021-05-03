using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event of type `GamePhasePair`. Inherits from `AtomEvent&lt;GamePhasePair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/GamePhasePair", fileName = "GamePhasePairEvent")]
    public sealed class GamePhasePairEvent : AtomEvent<GamePhasePair> { }
}
