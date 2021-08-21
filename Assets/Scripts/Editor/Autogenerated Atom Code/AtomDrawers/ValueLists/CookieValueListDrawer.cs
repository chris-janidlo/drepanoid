#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `Cookie`. Inherits from `AtomDrawer&lt;CookieValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(CookieValueList))]
    public class CookieValueListDrawer : AtomDrawer<CookieValueList> { }
}
#endif
