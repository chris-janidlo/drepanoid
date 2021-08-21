#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using Drepanoid;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `Cookie`. Inherits from `AtomEventEditor&lt;Cookie, CookieEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(CookieEvent))]
    public sealed class CookieEventEditor : AtomEventEditor<Cookie, CookieEvent> { }
}
#endif
