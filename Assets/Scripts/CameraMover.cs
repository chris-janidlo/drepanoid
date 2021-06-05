using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using crass;

public class CameraMover : MonoBehaviour
{
    [Tooltip("Refers to the distance the camera needs to travel, not the distance from the camera to the position it's tracking.")]
    public AnimationCurve TrackingFollowTimeByTravelDistance;
    public Vector2 FollowClampBoxExtents, MinimumDistanceToFollowBoxExtents;

    public float SceneTransitionMovementOffset;
    public EasingFunction.Ease SceneLoadEase, SceneUnloadEase;
    public float SceneLoadTransitionTime;

    public Vector2Variable CameraTrackingPosition, SceneChangeDirection;
    public SceneTransitionHelper SceneTransitionHelper;

    Vector3 sceneTransitionOffset => SceneTransitionMovementOffset * SceneChangeDirection.Value;

    TransitionableVector2 sceneChangeMovementTransition;
    Vector2 smoothFollowVelocity, previouslyTrackedTarget;
    float zPosition;

    void Start ()
    {
        CameraTrackingPosition.Value = transform.position;
        zPosition = transform.position.z;

        sceneChangeMovementTransition = new TransitionableVector2();
        sceneChangeMovementTransition.AttachMonoBehaviour(this);
        sceneChangeMovementTransition.FlashFromTo(-sceneTransitionOffset, Vector2.zero, SceneLoadTransitionTime, SceneLoadEase);
    }

    void Update ()
    {
        Vector3 newPos;

        if (sceneChangeMovementTransition.Transitioning)
        {
            newPos = sceneChangeMovementTransition.Value;
        }
        else
        {
            Vector2 target = getFollowTarget();
            float distance = Vector2.Distance(transform.position, target);
            float time = TrackingFollowTimeByTravelDistance.Evaluate(distance);

            newPos = Vector2.SmoothDamp(transform.position, target, ref smoothFollowVelocity, time);
        }

        newPos.z = zPosition;
        transform.position = newPos;
    }

    public void OnLevelGoalReached ()
    {
        sceneChangeMovementTransition.FlashFromTo(transform.position, transform.position + sceneTransitionOffset, SceneTransitionHelper.LevelUnloadAnimationTime, SceneUnloadEase);
    }

    Vector2 getFollowTarget ()
    {
        bool updateX, updateY;

        // detect if camera tracking target has snapped to a new position, and not smoothly moved to that new position ('1' is an arbitrary value that should encompass that)
        if (Vector2.Distance(CameraTrackingPosition.Value, CameraTrackingPosition.OldValue) > 1)
        {
            updateX = updateY = true;
        }
        else
        {
            Vector2 difference = CameraTrackingPosition.Value - (Vector2) transform.position;
            updateX = Mathf.Abs(difference.x) >= MinimumDistanceToFollowBoxExtents.x;
            updateY = Mathf.Abs(difference.y) >= MinimumDistanceToFollowBoxExtents.y;
        }

        Vector2 target = new Vector2
        (
            updateX ? CameraTrackingPosition.Value.x : previouslyTrackedTarget.x,
            updateY ? CameraTrackingPosition.Value.y : previouslyTrackedTarget.y
        );

        if (updateX || updateY) previouslyTrackedTarget = target;

        return new Vector2
        (
            Mathf.Clamp(target.x, -FollowClampBoxExtents.x, FollowClampBoxExtents.x),
            Mathf.Clamp(target.y, -FollowClampBoxExtents.y, FollowClampBoxExtents.y)
        );
    }
}
