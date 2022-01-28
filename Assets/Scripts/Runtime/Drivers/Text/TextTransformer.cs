using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    public abstract class TextTransformer : ScriptableObject
    {
        public abstract string Transform (string input);
    }
}
