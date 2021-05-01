#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `CookiePair`. Inherits from `AtomEventEditor&lt;CookiePair, CookiePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(CookiePairEvent))]
    public sealed class CookiePairEventEditor : AtomEventEditor<CookiePair, CookiePairEvent> { }
}
#endif
