using UnityEngine;

public class HealingZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHead"))
        {
            JSAM.AudioManager.PlaySound(AudioLibrarySounds.Heal);
            PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();
            playerHealth.SetHealth(playerHealth.BaseHealth);
            playerHealth.CanTakeDamage = false;
            playerHealth.SetPlayerSpriteColor(Color.magenta);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHead"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();
            playerHealth.CanTakeDamage = true;
            playerHealth.SetPlayerSpriteColor(Color.white);
        }
    }
}
