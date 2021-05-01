#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `CookiePair`. Inherits from `AtomDrawer&lt;CookiePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(CookiePairEvent))]
    public class CookiePairEventDrawer : AtomDrawer<CookiePairEvent> { }
}
#endif
