#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `GamePhase`. Inherits from `AtomDrawer&lt;GamePhaseEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(GamePhaseEvent))]
    public class GamePhaseEventDrawer : AtomDrawer<GamePhaseEvent> { }
}
#endif
