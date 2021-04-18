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

    public TranslationMover Mover;
    public SpriteRenderer Visual;

    bool wandering;
    Vector3 wanderLocalPosition;
    float wanderWaitTimer, returnToNormalColorTimer, bounceFlashTimer;

    void Start ()
    {
        wanderWaitTimer = RandomExtra.Range(TimeUntilWanderRange);
    }

    void Update ()
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
            Visual.transform.localPosition = Vector3.zero;
        }

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

    public void OnBounce ()
    {
        bounceFlashTimer = BounceFlashTime;
    }

    IEnumerator wander ()
    {
        while (true)
        {
            wanderLocalPosition = Random.insideUnitCircle * WanderRadius;
            yield return new WaitForSeconds(RandomExtra.Range(TimeBetweenWanderingsRange));
        }
    }
}
