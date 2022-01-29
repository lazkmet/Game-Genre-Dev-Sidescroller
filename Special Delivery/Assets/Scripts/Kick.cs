using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Kick : MonoBehaviour
{
    public Collider2D kickbox { get; private set; }
    public LayerMask enemyLayer;
    public float kickDuration;
    public float kickForce;
    public float stunTime;
    public float cooldown;
    private float currentCooldown;

    private void Awake()
    {
        kickbox = gameObject.GetComponent<Collider2D>();
    }
    private void FixedUpdate()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.fixedDeltaTime;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D enemy = collision.attachedRigidbody;
        if (enemy.velocity.y > 0)
        {
            Vector2 newVelocity = enemy.velocity;
            newVelocity.y = newVelocity.y * 0.25f;
            enemy.velocity = newVelocity;
        }
        Vector2 force = enemy.position - (Vector2)gameObject.transform.position;
        force = force.normalized * kickForce;
        enemy.AddForce(force, ForceMode2D.Impulse);
        try
        {
            GetComponentInParent<Rigidbody2D>().AddForce(-force, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<EnemyBehavior>().Stun(stunTime);
        }
        catch (System.Exception) { 
        
        }
    }
    public void TryKick() {
        if (!(currentCooldown > 0)) {
            StartCoroutine("KickTime");
        }
    }
    public IEnumerator KickTime() {
        currentCooldown = 0.2f + kickDuration + cooldown;
        yield return new WaitForSeconds(0.2f);
        kickbox.enabled = true;
        CheckArea();
        yield return new WaitForSeconds(kickDuration);
        kickbox.enabled = false;
    }
    public void CheckArea() {
        Collider2D[] enemiesHit = new Collider2D[10];
        ContactFilter2D castValues = new ContactFilter2D();
        castValues.SetLayerMask(enemyLayer);
        kickbox.OverlapCollider(castValues, enemiesHit);
        for (int i = 0; i < enemiesHit.Length; i++)
        {
            if (enemiesHit[i] != null)
            {
                OnTriggerEnter2D(enemiesHit[i]);
            }
        }
    }
    public void StopKick()
    {
        StopAllCoroutines();
        kickbox.enabled = false;
        currentCooldown = 0;
    }
}
