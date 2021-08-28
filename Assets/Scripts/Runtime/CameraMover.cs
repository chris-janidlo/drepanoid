using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using crass;

namespace Drepanoid
{
    public class CameraMover : MonoBehaviour
    {
        public Camera Camera;

        [Header("Position Tracking")]
        [Tooltip("Refers to the distance the camera needs to travel, not the distance from the camera to the position it's tracking.")]
        public AnimationCurve TrackingFollowTimeByTravelDistance;
        public Vector2 MinimumDistanceToFollowBoxExtents;
        [Tooltip("Defines the region of the level that the camera stays inside")]
        public Transform LowerLeftClampingPoint, UpperRightClampingPoint;
        public Vector2Variable CameraTrackingPosition;
        [Tooltip("Camera's target position will instantly snap if the tracked position moves at least this much in a single frame")]
        public float CameraSnapTrackingDistanceThreshold;

        [Header("Scene Transitions")]
        public float SceneTransitionMovementOffset;
        public EasingFunction.Ease SceneLoadEase, SceneUnloadEase;
        public float SceneLoadTransitionTime;

        public float NormalFov, FlattenedFov;
        public TransitionableFloat SceneLoadFovTransition, SceneUnloadFovTransition;

        public float MobileZoomWaitTime;
        public TransitionableFloat MobileZoomTransition;

        public Vector2Variable SceneChangeDirection;
        public SceneTransitionHelper SceneTransitionHelper;

        [Header("Pixel Perfection")]
        public int AssetPixelsPerUnit;
        public Vector2Int ReferenceResolution;
        public float MobileZoomAmount;

        Vector3 sceneTransitionOffset => SceneTransitionMovementOffset * SceneChangeDirection.Value;

        Vector2 xyPlanePosition;
        float zDistanceFromOrigin;

        TransitionableVector2 sceneChangeMovementTransition;
        Vector2 smoothFollowVelocity, followTargetMemory;

        Vector2Int resolution;
        float fov, zoom;
        bool zoomChanged, ballBouncedOnPaddleThisFrame;

        IEnumerator Start ()
        {
            CameraTrackingPosition.Value = transform.position;

            sceneChangeMovementTransition = new TransitionableVector2();
            sceneChangeMovementTransition.AttachMonoBehaviour(this);
            sceneChangeMovementTransition.FlashFromTo(-sceneTransitionOffset, Vector2.zero, SceneLoadTransitionTime, SceneLoadEase);

            SceneLoadFovTransition.AttachMonoBehaviour(this);
            SceneLoadFovTransition.FlashFromTo(FlattenedFov, NormalFov);

            SceneUnloadFovTransition.AttachMonoBehaviour(this);

            zoom = 1;
#if UNITY_ANDROID || UNITY_IOS
            yield return new WaitForSeconds(MobileZoomWaitTime);
            MobileZoomTransition.AttachMonoBehaviour(this);
            MobileZoomTransition.FlashFromTo(zoom, MobileZoomAmount);

            while (MobileZoomTransition.Transitioning)
            {
                zoom = MobileZoomTransition.Value;
                zoomChanged = true;
                yield return null;
            }

            zoom = MobileZoomAmount;
            zoomChanged = true;
#else
            yield return null;
#endif
        }

        void Update ()
        {
            updateFov();
            updateZDistanceFromOrigin();
            updateXyPlanePosition();

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
            SceneUnloadFovTransition.FlashFromTo(NormalFov, FlattenedFov);
        }

        public void OnBallBouncedOnPaddle ()
        {
            ballBouncedOnPaddleThisFrame = true;
        }

        void updateFov ()
        {
            if (SceneLoadFovTransition.Transitioning)
            {
                Camera.fieldOfView = SceneLoadFovTransition.Value;
            }

            if (SceneUnloadFovTransition.Transitioning)
            {
                Camera.fieldOfView = SceneUnloadFovTransition.Value;
            }
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
            if (zoomChanged || resolution.x != Screen.width || resolution.y != Screen.height || fov != currentHorizontalFov)
            {
                zoomChanged = false;
                resolution = new Vector2Int(Screen.width, Screen.height);
                fov = currentHorizontalFov;
                zDistanceFromOrigin = getPixelPerfectZDistanceFromOrigin();
            }
        }

        static readonly int[] _getFollowTargetAxes = new int[] { 0, 1 }; // as in more than one axis
        Vector2 getFollowTarget ()
        {
            bool forceUpdate = ballBouncedOnPaddleThisFrame ||
                Vector2.Distance(CameraTrackingPosition.Value, CameraTrackingPosition.OldValue) > CameraSnapTrackingDistanceThreshold; // ball snapped position

            foreach (int i in _getFollowTargetAxes)
            {
                float trackingPosition = CameraTrackingPosition.Value[i],
                      currentPosition = transform.position[i];

                if (!forceUpdate && Mathf.Abs(trackingPosition - currentPosition) < MinimumDistanceToFollowBoxExtents[i]) continue;

                // http://answers.unity.com/answers/1638803/view.html
                float frustumAngle = (i == 0 ? fov : Camera.fieldOfView) / 2,
                      halfScreen = zDistanceFromOrigin * Mathf.Tan(frustumAngle * Mathf.Deg2Rad);

                float clampMin = Mathf.Min(LowerLeftClampingPoint.position[i] + halfScreen, currentPosition),
                      clampMax = Mathf.Max(UpperRightClampingPoint.position[i] - halfScreen, currentPosition),
                      clampedPosition = Mathf.Clamp(trackingPosition, clampMin, clampMax);

                followTargetMemory[i] = Mathf.Round(clampedPosition * AssetPixelsPerUnit) / AssetPixelsPerUnit;
            }

            ballBouncedOnPaddleThisFrame = false;

            return followTargetMemory;
        }

        float getPixelPerfectZDistanceFromOrigin ()
        {
            float pixelMultiplier = Mathf.Max(1, Mathf.Min
            (
                resolution.x / ReferenceResolution.x,
                resolution.y / ReferenceResolution.y
            ));

            float frustrumWidthShouldBe = resolution.x / AssetPixelsPerUnit / pixelMultiplier / zoom;
            float innerFrustrumAngles = (180f - fov) / 2f * Mathf.Deg2Rad;
            float distanceFromOrigin = Mathf.Tan(innerFrustrumAngles) * frustrumWidthShouldBe / 2f;

            return distanceFromOrigin;
        }
    }
}