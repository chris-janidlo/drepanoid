using UnityEngine;

namespace Drepanoid.Drivers
{
    public class Driver : MonoBehaviour
    {
        public TextDriver TextDriverReference;
        public CharacterAnimationDriver CharacterAnimationDriverReference;
        public TranslationMovementDriver TranslationMovementReference;

        public static TextDriver Text => instance.TextDriverReference;
        public static CharacterAnimationDriver CharacterAnimations => instance.CharacterAnimationDriverReference;
        public static TranslationMovementDriver Mover => instance.TranslationMovementReference;

        static Driver instance;

        void Awake ()
        {
            instance = this;
        }
    }
}