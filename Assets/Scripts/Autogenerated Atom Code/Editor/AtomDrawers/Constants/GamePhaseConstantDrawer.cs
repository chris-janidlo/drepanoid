#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `GamePhase`. Inherits from `AtomDrawer&lt;GamePhaseConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(GamePhaseConstant))]
    public class GamePhaseConstantDrawer : VariableDrawer<GamePhaseConstant> { }
}
#endif
