using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;

public class MoveInputReader : MonoBehaviour
{
    public FloatVariable MoveAxis;

    public void OnMoveAxis (InputAction.CallbackContext context)
    {
        MoveAxis.Value = context.ReadValue<float>();
    }
}
