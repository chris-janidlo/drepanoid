using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class PaddleSegmentAnimator : MonoBehaviour
{
    public Color NormalColor, WanderColor;

    public float WanderRadius;
    public Vector2 TimeUntilWanderRange, TimeBetweenWanderingsRange;

    public TranslationMover Mover;
    public SpriteRenderer Visual;

    bool wandering;
    Vector3 wanderLocalPosition;
    float wanderWaitTimer;

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
        Visual.color = wandering ? WanderColor : NormalColor;

        Visual.transform.position = new Vector3
        (
            Mathf.Round(Visual.transform.position.x * 8) / 8,
            Mathf.Round(Visual.transform.position.y * 8) / 8,
            Mathf.Round(Visual.transform.position.z * 8) / 8
        );
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
