using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    private enum PowerUp
    {
        doubleJump, attack, dash, stomp
    };

    [SerializeField] PowerUp powerUp;

    private PlayerMovement playerMovement;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement = collision.GetComponent<PlayerMovement>();

            switch (powerUp)
            {
                case PowerUp.doubleJump:
                    playerMovement.hasDoubleJump = true;
                    break;
                case PowerUp.attack:
                    playerMovement.hasAttack = true;
                    break;
                case PowerUp.dash:
                    playerMovement.hasDash = true;
                    break;
                case PowerUp.stomp:
                    playerMovement.hasStomp = true;
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }

    }
}
