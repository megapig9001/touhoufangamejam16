using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] [Range(1, 10)]
    private int baseHealth = 3;
    public int BaseHealth { get => baseHealth; }

    private int currentHealth;
    public int CurrentHealth { get => currentHealth; }

    [SerializeField] [Range(0, 10f)]
    private float playerInvulnerabilitySeconds = 2;

    private Coroutine handlingOnHitInvulnerability;

    public bool CanTakeDamage { get; set; } = true;

    [SerializeField]
    private SpriteRenderer playerSpriteRenderer, hurtSpriteRenderer;
    [SerializeField]
    private GameObject playerFine, playerHurt;

    private void Start()
    {
        SetHealth(baseHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        JSAM.AudioManager.PlaySound(AudioLibrarySounds.Hit);
        if (!CanTakeDamage || handlingOnHitInvulnerability != null)
            return;

        TakeDamage(1);

        if (CurrentHealth != 0)
        {
            handlingOnHitInvulnerability = StartCoroutine(HandleInvulnerability());
        }
        else
        {
            new EventManager.PlayerDeathEvent().InvokeEvent();
        }
    }

    private IEnumerator HandleInvulnerability()
    {

        playerFine.SetActive(false);
        playerHurt.SetActive(true);
        hurtSpriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(playerInvulnerabilitySeconds);
        playerHurt.SetActive(false);
        playerFine.SetActive(true);

        handlingOnHitInvulnerability = null;
    }


    public void TakeDamage(int damageValue)
    {
        int newHealthValue = currentHealth - damageValue;

        if(newHealthValue <= 0)
        {
            SetHealth(0);
        }
        else
        {
            SetHealth(newHealthValue);
        }
    }

    /// <summary>
    /// Set the current health of the player. Equivalent to setting CurrentHealth = value
    /// </summary>
    /// <param name="value"></param>
    public void SetHealth(int value)
    {
        currentHealth = value > baseHealth ? baseHealth : value;

        EventManager.PlayerHealthChangeEvent e = new EventManager.PlayerHealthChangeEvent();
        e.newCurrentHealth = currentHealth;
        e.InvokeEvent();
    }


    public void ResetPlayer()
    {
        StopAllCoroutines();
        handlingOnHitInvulnerability = null;

        SetHealth(BaseHealth);
        playerSpriteRenderer.color = Color.white;
    }

    public void SetPlayerSpriteColor(Color color)
    {
        playerSpriteRenderer.color = color;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
