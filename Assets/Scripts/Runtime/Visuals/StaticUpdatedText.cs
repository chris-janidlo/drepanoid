using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using Drepanoid.Drivers;

namespace Drepanoid
{
    public class StaticUpdatedText : MonoBehaviour
    {
        public SetTextArguments TextArguments;
        public SetTextOptions TextOptions;

        IEnumerator Start ()
        {
            while (true)
            {
                yield return Driver.Text.SetText(TextArguments, TextOptions);
            }
        }
    }
}
