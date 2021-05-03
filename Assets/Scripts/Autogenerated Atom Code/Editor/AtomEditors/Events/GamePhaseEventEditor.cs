#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `GamePhase`. Inherits from `AtomEventEditor&lt;GamePhase, GamePhaseEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(GamePhaseEvent))]
    public sealed class GamePhaseEventEditor : AtomEventEditor<GamePhase, GamePhaseEvent> { }
}
#endif
