using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;

public class MoveInputReader : MonoBehaviour
{
    public float LevelLoadDelay; // may be faster than the level load animation
    public FloatVariable MoveAxis;

    bool readEnabled;

    IEnumerator Start ()
    {
        yield return new WaitForSeconds(LevelLoadDelay);
        readEnabled = true;
    }

    public void OnMoveAxis (InputAction.CallbackContext context)
    {
        if (readEnabled) MoveAxis.Value = context.ReadValue<float>();
    }
}
