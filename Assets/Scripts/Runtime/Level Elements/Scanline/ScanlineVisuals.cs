using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace Drepanoid
{
    public class ScanlineVisuals : MonoBehaviour
    {
        public float MinDistanceToTurnOnParticles, HorizontalLineVerticalOffset;

        public Transform Levitator, LeftAnchor, RightAnchor, LevitatorLineConnector;
        public ParticleSystem LevitatorParticles;
        public LineRenderer HorizontalLine, VerticalLine;
        public Vector2Variable CameraTrackingPosition;

        void Start ()
        {
            setLevitatorState(false);

            Vector3 offset = Vector3.up * HorizontalLineVerticalOffset;
            HorizontalLine.SetPositions(new Vector3[] { LeftAnchor.position + offset, RightAnchor.position + offset });
        }

        void FixedUpdate ()
        {
            Levitator.position = transform.position;

            Vector3 verticalLineStart = new Vector3(transform.position.x, LeftAnchor.position.y, LeftAnchor.position.z);
            VerticalLine.SetPosition(0, verticalLineStart);
            VerticalLine.SetPosition(1, LevitatorLineConnector.position);

            bool particlesOn = Vector2.Distance(transform.position, CameraTrackingPosition.Value) < MinDistanceToTurnOnParticles;
            setLevitatorState(particlesOn);
        }

        void setLevitatorState (bool on)
        {
            if (LevitatorParticles.isPlaying == on) return;

            if (on) LevitatorParticles.Play();
            else LevitatorParticles.Stop();
        }
    }
}
