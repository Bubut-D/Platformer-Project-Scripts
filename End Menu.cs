using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private Animator transition;

    public void EndGame()
    {
        transition.SetTrigger("Start");
        Invoke("Quit", 1.5f);
    }
    private void Quit()
    {
        Application.Quit();
    }

}
