using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class PaddleSegmentAnimator : MonoBehaviour
{
    public Color NormalColor, WanderColor, BounceFlashColor;

    public float BounceFlashTime;

    public float WanderRadius;
    public Vector2 TimeUntilWanderRange, TimeBetweenWanderingsRange;

    public TranslationMover Mover;
    public SpriteRenderer Visual;

    bool wandering;
    Vector3 wanderLocalPosition;
    float wanderWaitTimer, bounceFlashTimer;

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
            }
        }

        Visual.transform.localPosition = wandering ? wanderLocalPosition : Vector3.zero;

        Visual.transform.position = new Vector3
        (
            Mathf.Round(Visual.transform.position.x * 8) / 8,
            Mathf.Round(Visual.transform.position.y * 8) / 8,
            Mathf.Round(Visual.transform.position.z * 8) / 8
        );

        if (bounceFlashTimer > 0)
        {
            Visual.color = BounceFlashColor;
        }
        else
        {
            Visual.color = wandering ? WanderColor : NormalColor;
        }
        bounceFlashTimer -= Time.deltaTime;
    }

    void OnCollisionEnter2D (Collision2D collision)
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
