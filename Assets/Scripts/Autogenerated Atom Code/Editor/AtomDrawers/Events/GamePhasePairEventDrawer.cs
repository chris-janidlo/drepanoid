#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `GamePhasePair`. Inherits from `AtomDrawer&lt;GamePhasePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(GamePhasePairEvent))]
    public class GamePhasePairEventDrawer : AtomDrawer<GamePhasePairEvent> { }
}
#endif
