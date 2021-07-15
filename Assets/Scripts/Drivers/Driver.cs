using UnityEngine;

namespace Drepanoid.Drivers
{
    public class Driver : MonoBehaviour
    {
        public TextDriver TextDriverReference;
        public CharacterAnimationDriver CharacterAnimationDriverReference;

        public static TextDriver Text => instance.TextDriverReference;
        public static CharacterAnimationDriver CharacterAnimations => instance.CharacterAnimationDriverReference;

        static Driver instance;

        void Awake ()
        {
            instance = this;
        }
    }
}