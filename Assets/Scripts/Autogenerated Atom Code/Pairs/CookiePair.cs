using System;
using UnityEngine;
namespace UnityAtoms
{
    /// <summary>
    /// IPair of type `&lt;Cookie&gt;`. Inherits from `IPair&lt;Cookie&gt;`.
    /// </summary>
    [Serializable]
    public struct CookiePair : IPair<Cookie>
    {
        public Cookie Item1 { get => _item1; set => _item1 = value; }
        public Cookie Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private Cookie _item1;
        [SerializeField]
        private Cookie _item2;

        public void Deconstruct(out Cookie item1, out Cookie item2) { item1 = Item1; item2 = Item2; }
    }
}