using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteManager))]
public class EnemyBehavior : MonoBehaviour
{
    public Rigidbody2D body { get; private set; }
    public SpriteManager sprites { get; private set; }
    public Vector2 dimensions { get; private set; }
    public Collider2D player { get; private set; }
    public Collider2D targetArea;
    public LayerMask groundLayer;
    [HideInInspector]
    public bool movementEnabled;
    public float walkSpeed;
    public float impactForce;
    public float stunDuration;
    public float jumpAngle;
    public float jumpForce;
    private bool facingRight;
    private Vector2 startPos;
    private bool jumping;
    private bool stunned;
    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        sprites = gameObject.GetComponent<SpriteManager>();
        startPos = gameObject.transform.position;
        player = GameObject.Find("Player").GetComponent<Collider2D>();
        dimensions = body.gameObject.GetComponent<Collider2D>().bounds.size;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        Reset();
    }
    void Update()
    {
        if (movementEnabled && IsGroundedAt(body.transform.position)) {
            WalkForward();
        }
    }
    public void WalkForward() {
        Vector2 newPos = body.position;
        newPos += (facingRight ? Vector2.right : Vector2.left) * walkSpeed * Time.deltaTime;
        if (TraversableAt(newPos))
        {
            body.position = newPos;
            CancelInvoke();
        }
        else {
            facingRight = !facingRight;
            Invoke("Reset", 5f);
        }
    }
    public bool TraversableAt(Vector2 newPosition) {
        bool returnValue = false;
        Vector2 bufferPosition = newPosition;
        bufferPosition.x += (facingRight ? 1 : -1) * dimensions.x / 2 * 1.5f;
        RaycastHit2D hit = Physics2D.BoxCast(newPosition, dimensions, 0, (facingRight ? Vector2.right : Vector2.left ), dimensions.x/2 * 1.05f);
        if (hit.collider == null && IsGroundedAt(bufferPosition)) {
            returnValue = true;
        }
        return returnValue;
    }
    public bool IsGroundedAt(Vector2 position)
    {
        Collider2D coll = body.gameObject.GetComponent<Collider2D>();
        float distToBottom = (coll.bounds.center.y - coll.bounds.min.y) * 1.05f;
        bool returnValue = false;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, distToBottom, groundLayer);
        if (hit.collider != null)
        {
           returnValue = true;     
           body.velocity = body.velocity * 0.95f;      
        }
        return returnValue;
    }
    private IEnumerator CheckArea() {
        for (int i = 0; i < 10; i++) {
            yield return null;
        }
        if (targetArea.IsTouching(player)) {
            StartCoroutine("Jump");
        }
        else {
            StartCoroutine("CheckArea");
        }
    }
    private IEnumerator Jump() { 
        if (!jumping) {
            jumping = true;
            body.constraints = RigidbodyConstraints2D.None;
            movementEnabled = false;
            yield return new WaitForSeconds(0.5f);
            Vector2 bodyToTarget = ((Vector2)player.gameObject.transform.position - body.position);    
            float theta = Mathf.Clamp(90 + Vector2.SignedAngle(Vector2.up, bodyToTarget.normalized), 90 - jumpAngle, 90 + jumpAngle);
            theta = theta * Mathf.PI / 180;
            Vector2 jumpDirection = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            body.AddForce(jumpForce * jumpDirection * bodyToTarget.magnitude, ForceMode2D.Impulse);
            //first waits for initial grounding to cancel, then waits until grounded
            yield return new WaitWhile(() => IsGroundedAt(body.transform.position) == true);
            yield return new WaitUntil(() => IsGroundedAt(body.transform.position) == true);
            body.rotation = 0;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            yield return ("StunForSeconds", 0.5f);

            jumping = false;
        }
        StartCoroutine("CheckArea");
    }
    public void Stun(float stunDuration) {
        print("stunned");
        if (!stunned) {
            movementEnabled = false;
            stunned = true;
            StartCoroutine("StunForSeconds", stunDuration);
        }
    }
    public IEnumerator StunForSeconds( float stunTime) {
        yield return new WaitForSeconds(stunTime);
        movementEnabled = true;
        stunned = false;
    }
    
        
    
    public void Reset()
    {
        gameObject.transform.position = startPos;
        body.velocity = Vector2.zero;
        facingRight = true;
        movementEnabled = true;
        jumping = false;
        stunned = false;
        StopAllCoroutines();
        StartCoroutine("CheckArea");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7 && !stunned) {
            Rigidbody2D player = collision.collider.attachedRigidbody;
            if (player.gameObject.GetComponent<PlayerMovement>().movementEnabled) {
                if (player.velocity.y > 0) {
                    Vector2 newVelocity = player.velocity;
                    newVelocity.y = newVelocity.y * 0.25f;
                    player.velocity = newVelocity;
                }
                Vector2 force = player.position - (Vector2)gameObject.transform.position;
                force = force.normalized * impactForce;
                player.gameObject.GetComponent<PlayerMovement>().Stun(stunDuration);
                player.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}
