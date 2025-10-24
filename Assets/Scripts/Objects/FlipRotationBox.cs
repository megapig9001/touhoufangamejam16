using UnityEngine;

public class FlipRotationBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();
            player.ReverseRotation();
        }
    }
}
