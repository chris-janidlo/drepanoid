#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `GamePhase`. Inherits from `AtomDrawer&lt;GamePhaseVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(GamePhaseVariable))]
    public class GamePhaseVariableDrawer : VariableDrawer<GamePhaseVariable> { }
}
#endif
