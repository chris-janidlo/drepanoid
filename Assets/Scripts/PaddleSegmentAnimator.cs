using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class PaddleSegmentAnimator : MonoBehaviour
{
    public Color NormalColor, WanderColor, BounceFlashColor;

    public float BounceFlashTime;

    public float WanderRadius;
    public Vector2 TimeUntilWanderRange, TimeBetweenWanderingsRange, TimeToReturnToNormalColorRange;
    
    public TransitionableFloat MoveLagTransition;

    public TranslationMover Mover;
    public SpriteRenderer Visual;

    bool wandering;
    Vector3 wanderLocalPosition;
    float wanderWaitTimer, returnToNormalColorTimer, bounceFlashTimer;

    Vector3 startingVisualLocalPosition, visualPositionAtStartOfLag;
    int previousMoveDirection;

    void Start ()
    {
        MoveLagTransition.AttachMonoBehaviour(this);
        MoveLagTransition.Value = 1;

        startingVisualLocalPosition = Visual.transform.localPosition;

        wanderWaitTimer = RandomExtra.Range(TimeUntilWanderRange);
    }

    void Update ()
    {
        manageWandering();
        if (!wandering) manageMoveLag();
        manageColor();
    }

    public void OnBounce ()
    {
        bounceFlashTimer = BounceFlashTime;
    }

    void manageMoveLag ()
    {
        var moveDirection = MathfExtra.TernarySign(Mover.Velocity);
        if (moveDirection != previousMoveDirection)
        {
            previousMoveDirection = moveDirection;

            if (moveDirection != 0)
            {
                visualPositionAtStartOfLag = transform.position;
                MoveLagTransition.FlashFromTo(0, 1);
            }
        }

        Visual.transform.position = Vector3.Lerp(visualPositionAtStartOfLag, transform.position + startingVisualLocalPosition, MoveLagTransition.Value);
    }

    void manageWandering ()
    {
        if (Mover.Velocity != 0)
        {
            wanderWaitTimer = RandomExtra.Range(TimeUntilWanderRange);

            if (wandering)
            {
                wandering = false;
                StopAllCoroutines();
            }
        }
        else
        {
            wanderWaitTimer -= Time.deltaTime;

            if (wanderWaitTimer <= 0 && !wandering)
            {
                wandering = true;
                StartCoroutine(wander());
                returnToNormalColorTimer = RandomExtra.Range(TimeToReturnToNormalColorRange);
            }
        }

        if (wandering)
        {
            Visual.transform.localPosition = wanderLocalPosition;

            Visual.transform.position = new Vector3
            (
                Mathf.Round(Visual.transform.position.x * 8) / 8,
                Mathf.Round(Visual.transform.position.y * 8) / 8,
                Mathf.Round(Visual.transform.position.z * 8) / 8
            );
        }
        else
        {
            returnToNormalColorTimer -= Time.deltaTime;
        }
    }

    IEnumerator wander ()
    {
        while (true)
        {
            wanderLocalPosition = Random.insideUnitCircle * WanderRadius;
            yield return new WaitForSeconds(RandomExtra.Range(TimeBetweenWanderingsRange));
        }
    }

    void manageColor ()
    {
        if (bounceFlashTimer > 0)
        {
            Visual.color = BounceFlashColor;
        }
        else
        {
            Visual.color = returnToNormalColorTimer > 0 ? WanderColor : NormalColor;
        }

        bounceFlashTimer -= Time.deltaTime;
    }
}
