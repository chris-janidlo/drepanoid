using System;
using UnityEngine;

namespace Drepanoid
{
    [CreateAssetMenu(menuName = "Text Transformers/Username", fileName = "newUsernameTransformer")]
    public class UsernameTransformer : TextTransformer
    {
        public override string Transform (string input)
        {
            return input.Replace("$username", Environment.UserName);
        }
    }
}
