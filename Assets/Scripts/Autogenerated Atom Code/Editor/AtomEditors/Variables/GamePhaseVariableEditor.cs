using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `GamePhase`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(GamePhaseVariable))]
    public sealed class GamePhaseVariableEditor : AtomVariableEditor<GamePhase, GamePhasePair> { }
}
