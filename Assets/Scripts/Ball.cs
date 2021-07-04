using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using crass;

public class Ball : MonoBehaviour
{
    private static int deathsSinceLastExplosionDeath;

    public Vector2 Velocity { get; set; }

    public float Gravity;
    [Tooltip("Abstract constant that contains every variable in the drag equation that isn't velocity (so it includes coefficient of drag, reference area, and fluid density, which are all held constant throughout the game)")]
    public float DragCoefficient;

    public TransitionableFloat SpawnScaleTransition, DeathScaleTransition;
    public int SpawnScaleTransitionRounding, NormalDeathScaleTarget, ExplosionDeathScaleTarget;
    public AnimationCurve ChanceForDeathToBeExplosionDeathByDeathsSinceLastExplosionDeath;
    public float DeathAnimationWaitTimeBeforeShrinking, VictoriousDespawnWaitTime;

    public float BounceSpinMultiplier;
    [Range(0, 1)]
    public float AngularDrag;

    public Vector2Variable CameraTrackingPosition;

    public float KillFloorY;
    public VoidEvent BallDied;

    public Sprite OnSprite, OffSprite;
    public SpriteRenderer SpriteRenderer;
    public Collider2D Collider;

    public CharacterLoadAnimations CharacterAnimationsForLevelTransitions;

    Vector3 initialPosition;
    float angularVelocity; // positive = clockwise, negative = counter-clockwise
    bool spriteIsOn;
    bool frozen, despawningVictoriously;

    void Start ()
    {
        SpawnScaleTransition.AttachMonoBehaviour(this);
        SpawnScaleTransition.FlashFromTo(0, 1);
        DeathScaleTransition.AttachMonoBehaviour(this);

        spriteIsOn = RandomExtra.Chance(.5f);
        flipSprite();

        initialPosition = transform.position;
    }

    void Update ()
    {
        float scale = frozen
            ? DeathScaleTransition.Value
            : Mathf.Round(SpawnScaleTransition.Value * SpawnScaleTransitionRounding) / SpawnScaleTransitionRounding;

        transform.localScale = Vector3.one * scale;
    }

    void FixedUpdate ()
    {
        if (frozen) return;
        else if (transform.position.y < KillFloorY) Kill();

        move();
        spin();

        CameraTrackingPosition.Value = transform.position;
    }

    void OnDestroy ()
    {
        CameraTrackingPosition.Value = initialPosition;
    }

    /// <summary>
    /// Change the ball's velocity and spin as a result of bouncing off of something like a paddle or wall.
    /// </summary>
    /// <param name="resultingVelocity">How fast, and in what direction, the ball should be travelling after this collision.</param>
    /// <param name="cardinalCollisionNormal">The cardinal direction (ie up, down, left, right) that most closely matches the normal of the collision.</param>
    public void Bounce (Vector2 resultingVelocity, Vector2Int cardinalCollisionNormal)
    {
        if (frozen) return;

        var validNormals = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        if (!validNormals.Contains(cardinalCollisionNormal))
        {
            throw new ArgumentException($"axisAlignedCollisionNormal must be one of {string.Join(", ", validNormals)} - {cardinalCollisionNormal} is invalid");
        }

        angularVelocity += cardinalCollisionNormal.x == 0
            ? Math.Sign(cardinalCollisionNormal.y) * resultingVelocity.x * BounceSpinMultiplier
            : -Math.Sign(cardinalCollisionNormal.x) * resultingVelocity.y * BounceSpinMultiplier;

        Velocity = resultingVelocity;

        flipSprite();
    }

    public void Kill ()
    {
        if (frozen) return;

        frozen = true;
        StartCoroutine(deathRoutine());
    }

    public void DespawnVictoriously ()
    {
        if (frozen || despawningVictoriously) return;

        despawningVictoriously = true;
        StartCoroutine(victoryRoutine());
    }

    void move ()
    {
        if (!despawningVictoriously)
        {
            Velocity += Vector2.down * Gravity * Time.deltaTime;

            var dragAcceleration = DragCoefficient * (Velocity.sqrMagnitude / 2) * -Velocity.normalized; // drag equation
            Velocity += dragAcceleration * Time.deltaTime;
        }

        transform.position += (Vector3) Velocity * Time.deltaTime;
    }

    void spin ()
    {
        transform.Rotate(angularVelocity * Vector3.back * Time.deltaTime);

        if (Mathf.Abs(angularVelocity) >= Time.deltaTime)
        {
            angularVelocity -= angularVelocity * AngularDrag * Time.deltaTime;
        }
        else
        {
            angularVelocity = 0;
        }
    }

    void flipSprite ()
    {
        spriteIsOn = !spriteIsOn;
        SpriteRenderer.sprite = spriteIsOn ? OnSprite : OffSprite;
    }

    IEnumerator deathRoutine ()
    {
        Collider.enabled = false;

        yield return new WaitForSeconds(DeathAnimationWaitTimeBeforeShrinking);

        float explosionChance = ChanceForDeathToBeExplosionDeathByDeathsSinceLastExplosionDeath.Evaluate(deathsSinceLastExplosionDeath);
        bool exploding = RandomExtra.Chance(explosionChance);

        float targetScale = exploding
            ? ExplosionDeathScaleTarget
            : NormalDeathScaleTarget;

        DeathScaleTransition.StartTransitionTo(targetScale);
        yield return new WaitWhile(() => DeathScaleTransition.Transitioning);

        deathsSinceLastExplosionDeath = exploding
            ? 0
            : deathsSinceLastExplosionDeath + 1;

        BallDied.Raise();
        Destroy(gameObject);
    }

    IEnumerator victoryRoutine ()
    {
        yield return new WaitForSeconds(VictoriousDespawnWaitTime);

        frozen = true;
        transform.rotation = Quaternion.identity;
        StartCoroutine(CharacterAnimationsForLevelTransitions.AnimateSpriteRendererUnload(0, SpriteRenderer));
    }
}
