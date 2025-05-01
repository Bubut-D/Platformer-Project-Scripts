using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private HealthBar hBar;
    private PlayerLife playerLife;

    public int starCount = 0;
    private void Start()
    {
        hBar = GetComponent<HealthBar>();
        playerLife = GetComponent<PlayerLife>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Carrot") && playerLife.health < 3)
        {
            Destroy(collision.gameObject);
            playerLife.health++;
            hBar.UpdateHealthBar();
        }
        
    }
}
