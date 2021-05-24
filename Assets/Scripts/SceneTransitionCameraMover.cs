using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using crass;

public class SceneTransitionCameraMover : MonoBehaviour
{
    public float MovementOffset;
    public EasingFunction.Ease StartEase, EndEase;
    public float StartTime;

    public Vector2Variable SceneChangeDirection;
    public SceneTransitionHelper SceneTransitionHelper;

    Vector3 offset => MovementOffset * SceneChangeDirection.Value;

    TransitionableVector2 movementTransition;
    float transformZ;

    void Start ()
    {
        transformZ = transform.position.z;

        movementTransition = new TransitionableVector2();
        movementTransition.AttachMonoBehaviour(this);
        movementTransition.FlashFromTo(transform.position - offset, transform.position, StartTime, StartEase);
    }

    void Update ()
    {
        transform.position = new Vector3
        (
            movementTransition.Value.x,
            movementTransition.Value.y,
            transformZ
        );
    }

    public void OnLevelGoalReached ()
    {
        movementTransition.FlashFromTo(transform.position, transform.position + offset, SceneTransitionHelper.LevelUnloadAnimationTime, EndEase);
    }
}
