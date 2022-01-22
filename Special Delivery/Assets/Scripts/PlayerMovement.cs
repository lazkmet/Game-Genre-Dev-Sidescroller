using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body { get; private set; }
    public Grapple grapple { get; private set; }
    public Vector2 startPoint { get; private set; }
    public float walkSpeed;
    public float swingForce;
    public GameObject package;
    private void Awake()
    {
        body = this.GetComponent<Rigidbody2D>();
        grapple = this.GetComponent<Grapple>();
        startPoint = this.gameObject.transform.position;
        ResetToPosition(startPoint);
    }

    private void Update()
    {
        if (grapple.attached == true)
        {
            if (Input.GetKey(KeyCode.W))
            {

            }
            else if (Input.GetKey(KeyCode.S))
            {

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
                body.AddForce(Vector2.left * swingForce);
                
            }
            else if (Input.GetKey(KeyCode.D))
            {
                body.AddForce(Vector2.right * swingForce);
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
            }
        }
        return returnValue;
    }
}
