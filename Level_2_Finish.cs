using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_2_Finish : MonoBehaviour
{
    [SerializeField] private GameObject finalCam;
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator transition;

    private float totalTime = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            finalCam.SetActive(true);
            pm.canMove = false;
            StartCoroutine("Cutscene");
        }
    }
    private IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(.5f);
        transition.SetTrigger("Start");
        while (totalTime < 1.5f)
        {
            rb.velocity = new Vector2(pm.moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            totalTime += Time.fixedDeltaTime;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
