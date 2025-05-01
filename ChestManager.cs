using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChestManager : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D coll;
    private GameObject realStar;
    private GameObject player;
    private float startPos;
    private bool inFrontOfChest = false;

    [SerializeField] private Text starCount;
    [SerializeField] private Sprite openState;
    [SerializeField] private GameObject star;
    [SerializeField] private float starMoveDistance = 1f;
    [SerializeField] private float exponent = 2;
    [SerializeField] private float starMoveSpeed;
    [SerializeField] private float starMoveTime;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inFrontOfChest = true;
            player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inFrontOfChest = false;
        }
    }
    private void Update()
    {
        if (inFrontOfChest && Input.GetButtonDown("Interact"))
        {
            inFrontOfChest = false;
            coll.enabled = false;
            sr.sprite = openState;
            startPos = transform.position.y + .5f;
            realStar = Instantiate(star, new Vector3(transform.position.x, startPos, transform.position.z), transform.rotation, transform);
            StartCoroutine(MoveStar());
        }   
    }
    private IEnumerator MoveStar()
    {

        for (float i = 0; i < starMoveTime; i += Time.deltaTime)
        {
            float t = (Time.time * starMoveSpeed) % 1f;
            realStar.GetComponent<Transform>().position = new Vector2(realStar.transform.position.x, startPos + starMoveDistance * Parabola(t, exponent));

            yield return new WaitForSeconds(Time.deltaTime);
        }
        Invoke("DeleteStar", .5f);
    }
    private void DeleteStar()
    {
        Destroy(realStar);
        player.GetComponent<ItemCollector>().starCount++;
        starCount.text = player.GetComponent<ItemCollector>().starCount.ToString();
    }
    private float Parabola(float t, float k)
    {
        return Mathf.Pow(4f * t * (1f - t), k);
    }
}
