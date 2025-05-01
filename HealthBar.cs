using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Sprite[] healthBar = new Sprite[4];
    [SerializeField] private Image image;

    private PlayerLife playerLife;
    private void Start()
    {
        playerLife = GetComponent<PlayerLife>();
        image.sprite = healthBar[3];
    }
    public void UpdateHealthBar()
    {
        image.sprite = healthBar[playerLife.health];
    }
}
