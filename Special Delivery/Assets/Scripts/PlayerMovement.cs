using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grapple)), RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body { get; private set; }
    public Grapple grapple { get; private set; }
    public Vector2 startPoint { get; private set; }
    public float walkSpeed;
    public float airSpeed;
    public float swingForce;
    public float extendAmount;
    public GameObject package;
    public bool movementEnabled;
    private void Awake()
    {
        movementEnabled = true;
        body = this.GetComponent<Rigidbody2D>();
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        grapple = this.GetComponent<Grapple>();
        startPoint = this.gameObject.transform.position;
        ResetToPosition(startPoint);
    }

    private void Update()
    {
        if (movementEnabled) {
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
                //REWORK PHYSICS IF TIME
                else if (Input.GetKey(KeyCode.A))
                {
                    body.AddForce(Vector2.left * swingForce);

                }
                else if (Input.GetKey(KeyCode.D))
                {
                    body.AddForce(Vector2.right * swingForce);
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
            else {
                if (Input.GetKey(KeyCode.A))
                {
                    body.AddForce(Vector2.left * airSpeed);

                }
                else if (Input.GetKey(KeyCode.D))
                {
                    body.AddForce(Vector2.right * airSpeed);
                }
            }
        }
    }
    public void ResetToPosition(Vector2 point) {
        this.gameObject.transform.position = point;
    }
    public bool isGrounded() {
        Collider2D coll = body.gameObject.GetComponent<Collider2D>();
        float distToBottom = coll.bounds.center.y - coll.bounds.min.y + 0.1f;
        bool returnValue = false;
        RaycastHit2D hit = Physics2D.Raycast(coll.bounds.center, Vector2.down, distToBottom);
        if (hit.collider != null) {
            returnValue = true;
            if (grapple.attached == false) {
                body.velocity = body.velocity * 0.95f;
                if (body.velocity.magnitude < 0.01 && Mathf.Abs(body.rotation) > 1) {
                    print("reset rotation");
                    Vector3 currentPosition = body.transform.position;
                    currentPosition.y = hit.point.y + (gameObject.transform.lossyScale.y/2);
                    body.transform.rotation = Quaternion.identity;
                    body.transform.position = currentPosition;
                }
            }
        }
        return returnValue;
    }
}
