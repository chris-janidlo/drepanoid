using UnityEditor;
using UnityAtoms.Editor;
using Drepanoid;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `Cookie`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(CookieVariable))]
    public sealed class CookieVariableEditor : AtomVariableEditor<Cookie, CookiePair> { }
}
