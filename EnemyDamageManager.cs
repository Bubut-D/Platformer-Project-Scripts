using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    private Collider2D coll;
    private Rigidbody2D rb;
    private Animator animator;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isDead = false;

    [SerializeField] private bool hasRigidBody = true;
    [SerializeField] private float bounce = 5f;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = hasRigidBody ? GetComponent<Rigidbody2D>() : null;
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isAttacking && Vector2.Angle(Vector2.down, collision.GetContact(0).normal) <= 67.5)
            {
                BouncePlayer(collision.gameObject);
                collision.gameObject.GetComponent<PlayerMovement>().isStomping = false;
                collision.gameObject.GetComponent<PlayerMovement>().dashLine.SetActive(false);
                EnemyDeath();
            }
            else
                collision.gameObject.GetComponent<PlayerLife>().LoseHealth(isAttacking || Vector2.Angle(Vector2.down, collision.GetContact(0).normal) <= 90);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerLife>().LoseHealth(true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AttackCollider"))
        {
            if (collision.gameObject.GetComponentInParent<PlayerMovement>().isAttacking)
                EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        isDead = true;
        coll.enabled = false;
        if (hasRigidBody)
            rb.bodyType = RigidbodyType2D.Static;
        animator.Play("Enemy_Death");
    }
    private void DeleteObject()
    {
        Destroy(gameObject);
    }
    private void BouncePlayer(GameObject player)
    {
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, bounce);
    }
}
