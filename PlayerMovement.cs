using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerLife pl;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask ladder;
    [SerializeField] private GameObject attackLine;
    [SerializeField] private GameObject attackCollider;
    [SerializeField] public GameObject dashLine;

    [HideInInspector] public float horizontalMove = 0f;
    private float verticalMove = 0f;
    private bool isCrouching = false;
    private bool isClimbing = false;
    private bool inFrontOfLadder = false;
    public bool hasAttack = false;
    [HideInInspector] public bool isAttacking = false;
    public bool hasDash = false;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool canMove = true;
    public bool hasStomp = false;
    [HideInInspector] public bool isStomping = false;
    private bool doubleJump = false;
    public bool hasDoubleJump = false;
    private bool check;

    public float moveSpeed = 300f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float crouchSpeed = .5f;
    [SerializeField] private float climbSpeed = 150f;
    [SerializeField] private float stompSpeed = 5f;
    [SerializeField] private float stompOffSet = 1f;
    [SerializeField] private float stompOffSetPower = 1f;
    [SerializeField] private float stompOffSetTime = 1f;
    [SerializeField] private float attackDuration = 1.5f;
    [SerializeField] private float dashDuration = 1.5f;
    [SerializeField] private float dashForce = 10f;

    enum AnimationState { idle, walk, crouch, jump, fall, hurt, climb, attack };

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        pl = GetComponent<PlayerLife>();
    }

    // Köprü cutscene inde dash atýp durunca çýkabiliyo köprü alanýndan :D. ???????????????

    private void Update()
    {
        if (canMove)
        {
            check = !isAttacking && !isClimbing && !isStomping;
            if (isStomping && isGrounded)
            {
                dashLine.SetActive(false);
                isStomping = false;
            }

            if (Input.GetButtonDown("Crouch") && hasStomp && !isGrounded && !inFrontOfLadder && check && rb.velocity.y >= -stompSpeed )
                StartCoroutine(Stomp());

            if (Input.GetButtonDown("Attack") && hasAttack && check)
                StartCoroutine(Attack());

            if (Input.GetButtonDown("Dash") && hasDash && !isDashing && check)
                StartCoroutine(Dash());

            horizontalMove = isStomping ? 0f : Input.GetAxisRaw("Horizontal");
            isCrouching = (isGrounded && !inFrontOfLadder && !isAttacking && Input.GetAxisRaw("Vertical") < 0f);



            if (inFrontOfLadder && Input.GetAxisRaw("Vertical") > 0f)
            {
                isClimbing = true;
                rb.gravityScale = 0;
            }
            if ((isClimbing && isGrounded && Input.GetAxisRaw("Vertical") < 0f) || (!inFrontOfLadder && !isDashing))
            {
                isClimbing = false;
                rb.gravityScale = 1;
            }

            if (/*inFrontOfLadder*/ isClimbing && !isAttacking)
            {
                verticalMove = Input.GetAxisRaw("Vertical");
            }

            if (Input.GetButtonDown("Jump") && (isGrounded || (doubleJump && hasDoubleJump)))
            {
                rb.velocity = (rb.velocity.y > jumpForce) ? rb.velocity : new Vector2(rb.velocity.x, jumpForce);
                doubleJump = !doubleJump;
                dashLine.SetActive(false);
                isStomping = false;
            }

            //if (stomped && !isStomping && rb.velocity.y >= -stompSpeed)
            //{
            //    rb.velocity = new Vector2(rb.velocity.x, -stompSpeed);
            //    isStomping = true;
            //}

        }
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            isGrounded = Physics2D.BoxCast(coll.bounds.min, new Vector2(coll.bounds.size.x, .1f), 0f, Vector2.down, .2f, jumpableGround);
            inFrontOfLadder = Physics2D.BoxCast(coll.bounds.center, coll.size, 0f, Vector2.down, 0, ladder);                                                // Burdaydým
            rb.velocity = isCrouching ? new Vector2(crouchSpeed * horizontalMove * moveSpeed * Time.fixedDeltaTime, rb.velocity.y) : new Vector2(horizontalMove * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
            if (isClimbing)
                rb.velocity = new Vector2(0f, verticalMove * climbSpeed * Time.fixedDeltaTime);

        }
    }

    private IEnumerator Stomp()
    {
        isStomping = true;
        transform.DOJump(transform.position, stompOffSetPower, 1, stompOffSetTime);
        //rb.velocity = new Vector2(rb.velocity.x, stompOffSet);
        yield return new WaitForSeconds(stompOffSetTime * 0.4f);
        rb.velocity = new Vector2(rb.velocity.x, -stompSpeed);
        yield return new WaitForSeconds(.3f);
        dashLine.SetActive(true);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canMove = false;
        rb.gravityScale = 0;
        if (sr.flipX){
            transform.DORotate(new Vector3(0f, 0f, -90f), dashDuration * 0.1f);
            attackCollider.transform.DORotate(new Vector3(0f, 0f, 90f), dashDuration * 0.1f);
            rb.velocity = new Vector2(-dashForce, 0f);
        }
        else{
            transform.DORotate(new Vector3(0f, 0f, 90f), dashDuration * 0.1f);
            attackCollider.transform.DORotate(new Vector3(0f, 0f, -90f), dashDuration * 0.1f);
            rb.velocity = new Vector2(dashForce, 0f);
        }

        yield return new WaitForSeconds(dashDuration * 0.1f);
        dashLine.SetActive(true);
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(dashDuration * 0.9f);
        transform.DORotate(new Vector3(0f, 0f, 0f), dashDuration * 0.1f);
        attackCollider.transform.DORotate(new Vector3(0f, 0f, 0f), dashDuration * 0.1f);
        dashLine.SetActive(false);
        attackCollider.SetActive(false);
        yield return new WaitForSeconds(dashDuration * 0.1f);

        rb.gravityScale = 1;
        isDashing = false;
        canMove = true;
    }

    private IEnumerator Attack()
    {
        attackCollider.SetActive(true);
        isAttacking = true;
        attackLine.SetActive(true);
        //transform.DORotate(new Vector3(0f, 1080f, 0f), attackDuration, RotateMode.FastBeyond360);
        //attackLine.transform.DOLocalRotate(new Vector3(0f, -1080f, 0f), attackDuration, RotateMode.FastBeyond360);
        //attackCollider.transform.DOBlendableRotateBy(new Vector3(0f, -1080f, 0f), attackDuration, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(attackDuration);
        attackCollider.SetActive(false);
        isAttacking = false;
        attackLine.SetActive(false);
    }
    public void UpdateAnimation()
    {
        AnimationState state;
        
        if (horizontalMove < 0f)
        {
            state = AnimationState.walk;
            sr.flipX = true;
        }
        else if (horizontalMove > 0f)
        {
            sr.flipX = false;
            state = AnimationState.walk;
        }
        else
            state = AnimationState.idle;
        
        

        if (rb.velocity.y >.1f)
            state = AnimationState.jump;
        else if (rb.velocity.y < -.01f && !isGrounded )
            state = AnimationState.fall;

        if (isDashing)
            state = AnimationState.fall;

        if (isAttacking)
            state = AnimationState.attack;

        if (isCrouching)
            state = AnimationState.crouch;

        if (isClimbing)
            state = AnimationState.climb;

        if (pl.hurt)
            state = AnimationState.hurt;

        anim.SetInteger("State", (int)state);

        if (isClimbing && verticalMove == 0f)
            anim.speed = 0;
        else
            anim.speed = 1;
    }
}