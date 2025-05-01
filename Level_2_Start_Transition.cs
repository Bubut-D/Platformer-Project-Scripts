using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_2_Start_Transition : MonoBehaviour
{
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject virtualCam;
    [SerializeField] private GameObject player;
    private void Start()
    {
        virtualCam.SetActive(true);
        player.GetComponent<PlayerMovement>().canMove = false;
    }

    private void Update()
    {
        if (mainCam.transform.position == virtualCam.transform.position)
            StartCoroutine("Delete");
    }

    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(.1f);
        virtualCam.SetActive(false);
        player.GetComponent<PlayerMovement>().canMove = true;
        yield return new WaitForSeconds(1f);
        Destroy(virtualCam);
        Destroy(gameObject);
    }
}
