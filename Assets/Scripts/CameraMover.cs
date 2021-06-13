using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using crass;

public class CameraMover : MonoBehaviour
{
    public Camera Camera;

    [Header("Position Tracking")]
    [Tooltip("Refers to the distance the camera needs to travel, not the distance from the camera to the position it's tracking.")]
    public AnimationCurve TrackingFollowTimeByTravelDistance;
    public Vector2 FollowClampBoxExtents, MinimumDistanceToFollowBoxExtents;
    public Vector2Variable CameraTrackingPosition;

    [Header("Scene Transitions")]
    public float SceneTransitionMovementOffset;
    public EasingFunction.Ease SceneLoadEase, SceneUnloadEase;
    public float SceneLoadTransitionTime;
    public Vector2Variable SceneChangeDirection;
    public SceneTransitionHelper SceneTransitionHelper;

    [Header("Pixel Perfection")]
    public int AssetPixelsPerUnit;
    public Vector2Int ReferenceResolution;

    Vector3 sceneTransitionOffset => SceneTransitionMovementOffset * SceneChangeDirection.Value;

    Vector2 xyPlanePosition;
    float zDistanceFromOrigin;

    TransitionableVector2 sceneChangeMovementTransition;
    Vector2 smoothFollowVelocity, previouslyTrackedTarget;

    Vector2Int resolution;
    float fov;

    void Start ()
    {
        CameraTrackingPosition.Value = transform.position;

        sceneChangeMovementTransition = new TransitionableVector2();
        sceneChangeMovementTransition.AttachMonoBehaviour(this);
        sceneChangeMovementTransition.FlashFromTo(-sceneTransitionOffset, Vector2.zero, SceneLoadTransitionTime, SceneLoadEase);
    }

    void Update ()
    {
        updateXyPlanePosition();
        updateZDistanceFromOrigin();

        Camera.transform.position = new Vector3
        (
            xyPlanePosition.x,
            xyPlanePosition.y,
            -zDistanceFromOrigin
        );
    }

    public void OnLevelGoalReached ()
    {
        sceneChangeMovementTransition.FlashFromTo(transform.position, transform.position + sceneTransitionOffset, SceneTransitionHelper.LevelUnloadAnimationTime, SceneUnloadEase);
    }

    void updateXyPlanePosition ()
    {
        if (sceneChangeMovementTransition.Transitioning)
        {
            xyPlanePosition = sceneChangeMovementTransition.Value;
        }
        else
        {
            Vector2 target = getFollowTarget();
            float distance = Vector2.Distance(transform.position, target);
            float time = TrackingFollowTimeByTravelDistance.Evaluate(distance);

            xyPlanePosition = Vector2.SmoothDamp(transform.position, target, ref smoothFollowVelocity, time);
        }
    }

    void updateZDistanceFromOrigin ()
    {
        float currentHorizontalFov = Camera.VerticalToHorizontalFieldOfView(Camera.fieldOfView, (float) Screen.width / Screen.height);
        if (resolution.x != Screen.width || resolution.y != Screen.height || fov != currentHorizontalFov)
        {
            resolution = new Vector2Int(Screen.width, Screen.height);
            fov = currentHorizontalFov;
            zDistanceFromOrigin = getPixelPerfectZDistanceFromOrigin();
        }
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

        Vector2 clampedTarget = new Vector2
        (
            Mathf.Clamp(target.x, -FollowClampBoxExtents.x, FollowClampBoxExtents.x),
            Mathf.Clamp(target.y, -FollowClampBoxExtents.y, FollowClampBoxExtents.y)
        );

        return new Vector2
        (
            Mathf.Round(clampedTarget.x * AssetPixelsPerUnit) / AssetPixelsPerUnit,
            Mathf.Round(clampedTarget.y * AssetPixelsPerUnit) / AssetPixelsPerUnit
        );
    }

    float getPixelPerfectZDistanceFromOrigin ()
    {
        float pixelMultiplier = Mathf.Max(1, Mathf.Min
        (
            resolution.x / ReferenceResolution.x,
            resolution.y / ReferenceResolution.y
        ));

        float frustrumWidthShouldBe = resolution.x / AssetPixelsPerUnit / pixelMultiplier;
        float innerFrustrumAngles = (180f - fov) / 2f * Mathf.Deg2Rad;
        float distanceFromOrigin = Mathf.Tan(innerFrustrumAngles) * frustrumWidthShouldBe / 2f;

        return distanceFromOrigin;
    }
}
