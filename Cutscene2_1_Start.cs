using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cutscene2_1_Start : MonoBehaviour
{
    [SerializeField] private GameObject[] platforms = new GameObject[13];
    [SerializeField] private GameObject[] slugs = new GameObject[2];
    [SerializeField] private Transform[] waypoints = new Transform[2];
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cam;
    [SerializeField] private float moveDuration;
    [SerializeField] private float turnDuration;

    private Tween myTween1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().canMove = false;
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<PlayerMovement>().horizontalMove = 0;
            collision.GetComponent<PlayerMovement>().isGrounded = false;
            for(int i = 0; i < 2; i++)
            {
                slugs[i].SetActive(true);
                slugs[i].transform.DOMoveX(waypoints[i].position.x, moveDuration).SetEase(Ease.OutSine);
            }
            Invoke("BeginCutscene", moveDuration - 0.5f);
        }
    }

    private void BeginCutscene()
    {
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
        foreach (GameObject platform in platforms)
            Destroy(platform);
        foreach (var slug in slugs)
            slug.transform.DORotate(new Vector3(0f, 0f, 180f), turnDuration).SetLoops(-1,LoopType.Incremental).SetEase(Ease.Linear);
        player.transform.DOMoveY(-71.5f, 3f).SetEase(Ease.InQuad);
        myTween1 = player.transform.DORotate(new Vector3(0f, 0f, 360f), turnDuration * 5, RotateMode.FastBeyond360).SetAutoKill(false).SetEase(Ease.Linear);
        myTween1.OnComplete(() => myTween1.Restart());
        cam.SetActive(true);
        player.GetComponent<BoxCollider2D>().enabled = false;
        Invoke("EndCutscene", 2.95f);
    }

    private void EndCutscene()
    {
        myTween1.OnComplete(null);
        myTween1.SetAutoKill(true);
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<PlayerMovement>().canMove = true;
        cam.SetActive(false);
        foreach (var slug in slugs)
            slug.GetComponent<EnemyDamageManager>().EnemyDeath();
        Destroy(gameObject);
    }
}
