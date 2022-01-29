using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grapple)), RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body { get; private set; }
    public Grapple grapple { get; private set; }
    public Kick kick;
    public LayerMask terrainLayer;
    public LayerMask enemyLayer;
    public float walkSpeed;
    public float airSpeed;
    public float swingForce;
    public float extendAmount;
    public GameObject package;
    public bool movementEnabled;
    public bool hasPackage;
    public bool kicking;
    [HideInInspector]
    public GameManager manager;
    private void Awake()
    {
        body = this.GetComponent<Rigidbody2D>();
        grapple = this.GetComponent<Grapple>();
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Reset();
    }

    private void Update()
    {
        if (movementEnabled)
        {
            if (grapple.attached)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    grapple.Extend(-extendAmount);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    grapple.Extend(extendAmount);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    body.AddForce(Vector2.left * swingForce * Time.deltaTime);

                }
                else if (Input.GetKey(KeyCode.D))
                {
                    body.AddForce(Vector2.right * swingForce * Time.deltaTime);
                }
            }
            else if (isGrounded())
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Vector2 newPosition = body.position;
                    newPosition += Vector2.left * walkSpeed * Time.deltaTime;
                    body.position = newPosition;

                }
                else if (Input.GetKey(KeyCode.D))
                {
                    Vector2 newPosition = body.position;
                    newPosition += Vector2.right * walkSpeed * Time.deltaTime;
                    body.position = newPosition;

                }
            }
            else
            {
                if (Input.GetKey(KeyCode.A))
                {
                    body.AddForce(Vector2.left * airSpeed * Time.deltaTime);

                }
                else if (Input.GetKey(KeyCode.D))
                {
                    body.AddForce(Vector2.right * airSpeed * Time.deltaTime);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && !isGrounded()) {
                kick.TryKick();
            }
        }
        else {
            isGrounded();
        }
    }
    public void ResetToCheckpoint(Checkpoint point) {
        this.gameObject.transform.SetPositionAndRotation(point.position, Quaternion.identity);
        body.velocity = Vector2.zero;
        movementEnabled = true;
        hasPackage = point.packageHad;
        kick.StopKick();
    }
    public bool isGrounded() {
        Collider2D coll = body.gameObject.GetComponent<Collider2D>();
        float distToBottom = coll.bounds.center.y - coll.bounds.min.y + 0.1f;
        bool returnValue = false;
        RaycastHit2D hit = Physics2D.Raycast(coll.bounds.center, Vector2.down, distToBottom, terrainLayer);
        if (hit.collider != null) {
            if (grapple.attached == false) {
                returnValue = true;
                if (hasPackage && hit.collider.gameObject.name == "Win Zone") {
                    manager.Win();
                }
                else if (hasPackage && hit.collider.gameObject.tag != "safe")
                {
                    manager.Loss();
                }
                else
                {
                    body.velocity = body.velocity * 0.95f;
                    if (body.velocity.magnitude < 0.01 && Mathf.Abs(body.rotation) > 1)
                    {
                        Vector3 currentPosition = body.transform.position;
                        currentPosition.y = hit.point.y + (gameObject.transform.lossyScale.y / 2);
                        body.transform.rotation = Quaternion.identity;
                        body.transform.position = currentPosition;
                    }
                }
                
            }
        }
        return returnValue;
    }
    public void Reset()
    {
        CancelInvoke();
        try
        {
            ResetToCheckpoint(manager.startPoint);
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
        }
        hasPackage = false;
    }
    /*private void Kick() {
        kicking = true;
        if (movementEnabled)
        {
            kickbox.SetActive(true);
            StartCoroutine("DetectKick");
        }
        else {
            StartCoroutine("Cooldown", 0);
        }
    }
    public IEnumerator DetectKick() {
        yield return new WaitForSeconds(0.2f);
        float currentTime = 0;
        Collider2D[] enemiesHit = new Collider2D[10];
        ContactFilter2D castValues = new ContactFilter2D();
        castValues.SetLayerMask(enemyLayer);
        while (currentTime < kickDuration && !isGrounded()) {
            kickbox.GetComponent<Collider2D>().OverlapCollider(castValues, enemiesHit);
            for (int i = 0; i < enemiesHit.Length; i++) {
                if (enemiesHit[i] != null) { 
                    Rigidbody2D enemy = enemiesHit[i].attachedRigidbody;
                    if (enemy.velocity.y > 0){
                            Vector2 newVelocity = enemy.velocity;
                            newVelocity.y = newVelocity.y * 0.25f;
                            enemy.velocity = newVelocity;
                    }
                        Vector2 force = enemy.position - body.position;
                        force = force.normalized * kickForce;
                        enemy.gameObject.GetComponent<EnemyBehavior>().Stun(stunDuration);
                        enemy.AddForce(force, ForceMode2D.Impulse);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
        StartCoroutine("Cooldown", kickCooldown);
    }
    public IEnumerator Cooldown(float cooldownTime) {
        kickbox.SetActive(false);
        yield return new WaitForSeconds(cooldownTime);
        kicking = false;
    }
*/
    public void Stun(float stunTime) {
        movementEnabled = false;
        CancelInvoke();
        grapple.StartCoroutine("CR_ReturnRope");
        StartCoroutine("CR_WaitForStun", stunTime);
    }
    private IEnumerator CR_WaitForStun(float stunTime) {
        yield return new WaitForSeconds(stunTime);
        if (!manager.gameOver) {
            movementEnabled = true;
        }
    }
}
