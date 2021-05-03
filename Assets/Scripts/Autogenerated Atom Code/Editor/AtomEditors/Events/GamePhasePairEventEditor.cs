#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `GamePhasePair`. Inherits from `AtomEventEditor&lt;GamePhasePair, GamePhasePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(GamePhasePairEvent))]
    public sealed class GamePhasePairEventEditor : AtomEventEditor<GamePhasePair, GamePhasePairEvent> { }
}
#endif
