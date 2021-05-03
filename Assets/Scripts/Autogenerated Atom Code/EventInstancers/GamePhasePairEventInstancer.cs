using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Instancer of type `GamePhasePair`. Inherits from `AtomEventInstancer&lt;GamePhasePair, GamePhasePairEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/GamePhasePair Event Instancer")]
    public class GamePhasePairEventInstancer : AtomEventInstancer<GamePhasePair, GamePhasePairEvent> { }
}
