using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] [Range(1, 10)]
    private int baseHealth = 3;

    public int CurrentHealth { get; set; }

    [SerializeField] [Range(0, 10f)]
    private float playerInvulnerabilitySeconds = 2;

    private Coroutine handlingOnHitInvulnerability;

    [SerializeField]
    private SpriteRenderer playerSpriteRenderer;

    private void Awake()
    {
        CurrentHealth = baseHealth;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (handlingOnHitInvulnerability != null)
            return;

        TakeDamage(1);

        if (CurrentHealth == 0)
        {
            TempKillPlayer();
            return;
        }

        handlingOnHitInvulnerability = StartCoroutine(HandleInvulnerability());
    }

    private IEnumerator HandleInvulnerability()
    {

        playerSpriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(playerInvulnerabilitySeconds);

        playerSpriteRenderer.color = Color.white;
        handlingOnHitInvulnerability = null;
    }


    public void TakeDamage(int damageValue)
    {
        CurrentHealth -= damageValue;

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
        }
    }

    ///Temp code for early testing
    public Vector2 RespawnPosition { get; set; }

    private void Start()
    {
        RespawnPosition = transform.position;
    }
    private void TempKillPlayer()
    {
        StopAllCoroutines();

        PlayerController controller = GetComponent<PlayerController>();
        controller.StopAllCoroutines();
        controller.CurrentRotationAngle = 0;
        controller.PlayerInputDisabled = false;
        controller.RotationDisabled = false;

        transform.position = RespawnPosition;
        CurrentHealth = baseHealth;
    }
}
