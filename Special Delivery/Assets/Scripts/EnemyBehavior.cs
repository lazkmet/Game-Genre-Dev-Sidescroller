using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteManager))]
public class EnemyBehavior : MonoBehaviour
{
    public Rigidbody2D body { get; private set; }
    public SpriteManager sprites { get; private set; }
    [HideInInspector]
    public bool movementEnabled;
    public float walkSpeed;
    public float impactForce;
    public float stunDuration;
    public bool facingLeft;
    private Vector2 startPos;
    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        sprites = gameObject.GetComponent<SpriteManager>();
        startPos = gameObject.transform.position;
        Reset();
    }
    void Update()
    {
        if (movementEnabled) {
            WalkForward();
            //CheckArea();
        }
    }
    public void WalkForward() {
        //check if boxcast forward would hit collidable object, or if moving forward would
        //lead to falling
        if (true)
        {
            Vector2 newPos = body.position;
            newPos += (facingLeft ? Vector2.right : Vector2.left) * walkSpeed * Time.deltaTime;
            body.position = newPos;
        }
        else {
            facingLeft = !facingLeft;
        }
    }
    public void Reset()
    {
        gameObject.transform.position = startPos;
        facingLeft = true;
        movementEnabled = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7) {
            Rigidbody2D player = collision.collider.attachedRigidbody;
            if (player.velocity.y > 0) {
                Vector2 newVelocity = player.velocity;
                newVelocity.y = newVelocity.y * 0.25f;
                player.velocity = newVelocity;
            }
            Vector2 force = player.position - (Vector2)gameObject.transform.position;
            force = force.normalized * impactForce;
            player.gameObject.GetComponent<PlayerMovement>().Stun(stunDuration);
            player.AddForce(force);
        }
    }
}
