#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid.LevelBaking
{
    public interface IBakedGood
    {
        /// <summary>
        /// Bakes the changes in this object to the scene. Should be idempotent.
        /// </summary>
        void Bake ();

        /// <summary>
        /// Removes the result of baked changes from the scene and reverts the object to its pre-baked state. Should be idempotent.
        /// </summary>
        void Unbake ();
    }
}
#endif
