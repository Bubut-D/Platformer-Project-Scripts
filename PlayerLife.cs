using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;


public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D coll;
    private HealthBar hBar;
    private PlayerMovement pm;
    private int enemyLayer;

    [SerializeField] private CinemachineVirtualCamera cameraController;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private float invincibilityDurationSeconds = 1.5f;
    [SerializeField] private float invincibilityDeltaTime = .15f;
    [SerializeField] private float hurtTime = 1f;
    [SerializeField] private float hurtBounceHor = 5f;
    [SerializeField] private float hurtBounceVer = 5f;
    [SerializeField] private float deathTime = 2.9f;
    [SerializeField] private float deathBounce = 6f;

    [HideInInspector] public int health = 3;
    private bool invincible = false;
    [HideInInspector] public bool hurt = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        hBar = GetComponent<HealthBar>();
        pm = GetComponent<PlayerMovement>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
            LoseHealth(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AttackCollider"))
            LoseHealth(true);
    }

    public void LoseHealth(bool bounce)
    {
        if (invincible)
            return;            
        
        health--;
        hBar.UpdateHealthBar();
        if (health == 0)
        {
            Die();
            Debug.Log(string.Format("<color=#FF0000>Died</color>"));
            return;
        }

        StartCoroutine(HurtState(bounce));
        Debug.Log(string.Format("<color=#FF00FF>Hurt</color>"));
        StartCoroutine(BecomeTemporarilyInvincible());
    }
    private IEnumerator BecomeTemporarilyInvincible()
    {
        invincible = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer);

        Debug.Log(string.Format("<color=#0000FF>Invincible</color>"));

        for (float i = 0; i < invincibilityDurationSeconds; i += invincibilityDeltaTime)
        {           
            if (sr.enabled == true)
            {
                sr.enabled = false;
            }
            else
            {
                sr.enabled = true;
            }
            yield return new WaitForSeconds(invincibilityDeltaTime);
        }
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, false);
        sr.enabled = true;
        invincible = false;
        Debug.Log("Not Invincible");
        rb.WakeUp();
    }
    private IEnumerator HurtState(bool bounce)
    {
        hurt = true;
        pm.canMove = false;
        float verBounce = (bounce) ? hurtBounceVer : 0f;
        rb.velocity = (rb.velocity.x == 0f) ? new Vector2(-1f * hurtBounceHor, verBounce) : new Vector2((rb.velocity.x / Mathf.Abs(rb.velocity.x)) * -1f * hurtBounceHor, verBounce);
        yield return new WaitForSeconds(hurtTime);
        hurt = false;
        pm.canMove = true;
    }
    private void Die() {
        hurt = true;
        pm.canMove = false;
        StartCoroutine(DeathAnimation());
        Invoke("Restart", 3);
    }
    private void Restart()
    {
        gameOverMenu.SetActive(true);
    }
    private IEnumerator DeathAnimation()
    {
        coll.enabled = false;
        cameraController.enabled = false;
        sr.sortingLayerName = "Foreground";
        sr.sortingOrder = 10;
        rb.velocity = new Vector2(rb.velocity.x, deathBounce);
        yield return new WaitForSeconds(deathTime);
    }
}
