#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `GamePhase`. Inherits from `AtomDrawer&lt;GamePhaseValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(GamePhaseValueList))]
    public class GamePhaseValueListDrawer : AtomDrawer<GamePhaseValueList> { }
}
#endif
