using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform[] temp = new Transform[3];

    public int waypointIndex = 0;
    public float[] waypoints = new float[2];
    private float prevX;
    public bool canMove = true;

    [SerializeField] private bool isFlying = false;
    [SerializeField] private float floatAmplitude = 1f;
    [SerializeField] private float floatFrequency = 1f;
    [SerializeField] private float speed = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        temp = GetComponentsInChildren<Transform>();
        waypoints[0] = temp[1].position.x;
        waypoints[1] = temp[2].position.x;
        for (int i = 0; i < 2; i++)
            Destroy(temp[i + 1].gameObject);
        prevX = transform.position.x;
    }
    private void Update()
    {
        if (isFlying)
            transform.position = new Vector2(transform.position.x, Mathf.Sin(Time.time * floatFrequency) * floatAmplitude);
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            if (Vector2.Distance(transform.position, new Vector2(waypoints[waypointIndex], transform.position.y)) < .1f || prevX == transform.position.x)
            {
                waypointIndex++;
                waypointIndex %= 2;
                sr.flipX = waypointIndex == 0;
            }
            rb.velocity = new Vector2((1f - waypointIndex * 2) * Time.fixedDeltaTime * speed, rb.velocity.y);
            prevX = transform.position.x;
        } 
    }
}
