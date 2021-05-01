#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `Cookie`. Inherits from `AtomDrawer&lt;CookieConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(CookieConstant))]
    public class CookieConstantDrawer : VariableDrawer<CookieConstant> { }
}
#endif
