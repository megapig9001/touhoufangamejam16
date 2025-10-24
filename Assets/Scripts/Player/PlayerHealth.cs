using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] [Range(1, 10)]
    private int baseHealth = 3;
    public int BaseHealth { get => baseHealth; }


    private int CurrentHealth { get; set; }

    [SerializeField] [Range(0, 10f)]
    private float playerInvulnerabilitySeconds = 2;

    private Coroutine handlingOnHitInvulnerability;

    public bool CanTakeDamage { get; set; }

    [SerializeField]
    private SpriteRenderer playerSpriteRenderer;

    private void Awake()
    {
        CurrentHealth = BaseHealth;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!CanTakeDamage || handlingOnHitInvulnerability != null)
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
        Debug.Log($"CurrentHealth = {CurrentHealth}");
    }

    public void SetHealth(int value)
    {
        CurrentHealth = value > BaseHealth ? BaseHealth : value;
        Debug.Log($"CurrentHealth = {CurrentHealth}");
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
        CurrentHealth = BaseHealth;
    }

    public void SetPlayerSpriteColor(Color color)
    {
        playerSpriteRenderer.color = color;
    }
}
