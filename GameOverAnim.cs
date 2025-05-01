using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverAnim : MonoBehaviour
{
    [SerializeField] float interval = .15f;
    [SerializeField] Sprite[] spriteArr;
    private Image image;

    private int index = 0;

    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(Crouch());
    }

    IEnumerator Crouch() {
        yield return new WaitForSeconds(interval);
        image.sprite = spriteArr[index];
        index++;
        index %= spriteArr.Length;
        StartCoroutine(Crouch());
    }
}
