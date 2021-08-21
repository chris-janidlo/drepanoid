#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `Cookie`. Inherits from `AtomDrawer&lt;CookieVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(CookieVariable))]
    public class CookieVariableDrawer : VariableDrawer<CookieVariable> { }
}
#endif
