using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    [System.Serializable]
    public class BakeException : System.Exception
    {
        public BakeException() { }
        public BakeException(string message) : base(message) { }
        public BakeException(string message, System.Exception inner) : base(message, inner) { }
        protected BakeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
