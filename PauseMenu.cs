using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private GameObject pauseMenu;
    [HideInInspector] public bool paused;

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        paused = true;
    }
    public void UnpauseGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }
    public void QuitGame()
    {
        transition.SetTrigger("Start");
        StartCoroutine("LoadScene");
    }


    private IEnumerator LoadScene()
    {
        for (float i = 0f; i <= 1.5f; i += Time.unscaledDeltaTime)
            yield return null;

        SceneManager.LoadScene(0);
    }
}
