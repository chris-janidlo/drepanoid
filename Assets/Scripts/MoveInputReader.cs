using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;

namespace Drepanoid
{
    public class MoveInputReader : MonoBehaviour
    {
        public float LevelLoadDelay; // may be faster than the level load animation
        public FloatVariable MoveAxis;

        bool moveEnabled;
        float mostRecentMoveAxisInput;

        IEnumerator Start ()
        {
            yield return new WaitForSeconds(LevelLoadDelay);
            moveEnabled = true;
        }

        void Update ()
        {
            if (moveEnabled)
            {
                // want to allow touch and other input at the same time because:
                    // plenty of devices have both a touchscreen and other input
                    // want to support gamepads on phones
                    // windows devices don't play well with common ways of detecting if you're on mobile
                MoveAxis.Value = sideOfScreenBeingTouched() ?? mostRecentMoveAxisInput;
            }
        }

        public void OnMoveAxis (InputAction.CallbackContext context)
        {
            mostRecentMoveAxisInput = context.ReadValue<float>();
        }

        float? sideOfScreenBeingTouched ()
        {
            // we use the classic input system here because the new system doesn't support unity remote and that's a pain in the ass
            if (Input.touchCount == 0) return null;

            float touch = Input.GetTouch(0).position.x;
            return touch.CompareTo(Screen.width / 2f);
        }
    }
}