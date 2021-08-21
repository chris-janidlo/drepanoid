using UnityEngine;

namespace Drepanoid
{
    public class LevelNotes : MonoBehaviour
    {
        [TextArea(5, 500)]
        public string Notes;
    }
}