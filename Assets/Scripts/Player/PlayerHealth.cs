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
    [SerializeField] Sprite playerDefaultSprite;
    [SerializeField] Sprite playerHappySprite;
    [SerializeField] Sprite playerBadAppleSprite;


    [SerializeField]
    private GameObject playerFine, playerHurt;
    [SerializeField] Color hurtColor;

    [SerializeField]
    private ParticleSystem collisionEffect;

    private BadAppleMode badAppleMode;

    private void Awake()
    {
        //Handles sprite changes for Bad Apple Mode.
        badAppleMode = GetComponent<BadAppleMode>();
        if (badAppleMode.GetBadAppleMode())
        {
            playerDefaultSprite = playerBadAppleSprite;
            playerHappySprite = playerBadAppleSprite;
            playerSpriteRenderer.sprite = playerBadAppleSprite;
            hurtSpriteRenderer.sprite = playerBadAppleSprite;
        }
    }

    private void Start()
    {
        SetHealth(baseHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        JSAM.AudioManager.PlaySound(AudioLibrarySounds.Hit);
        PlayCollisionEffect(collision.GetContact(0).point);

        if (!CanTakeDamage || handlingOnHitInvulnerability != null)
            return;

        Vector2 collisionPoint = collision.GetContact(0).point;
        TakeDamage(1, collisionPoint.x, collisionPoint.y);

        if (CurrentHealth != 0)
        {
            handlingOnHitInvulnerability = StartCoroutine(HandleInvulnerability());
        }
        else
        {
            new EventManager.PlayerDeathEvent().InvokeEvent();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!CanTakeDamage || handlingOnHitInvulnerability != null)
            return;

        JSAM.AudioManager.PlaySound(AudioLibrarySounds.Hit);
        Vector2 collisionPoint = collision.GetContact(0).point;
        TakeDamage(1, collisionPoint.x, collisionPoint.y);

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
        hurtSpriteRenderer.color = hurtColor;

        yield return new WaitForSeconds(playerInvulnerabilitySeconds);
        playerHurt.SetActive(false);
        playerFine.SetActive(true);

        handlingOnHitInvulnerability = null;
    }


    public void TakeDamage(int damageValue, float collisionPosX = 0, float collisionPosY = 0)
    {
        int newHealthValue = currentHealth - damageValue;

        if(newHealthValue <= 0)
        {
            SetHealth(0);
        }
        else
        {
            SetHealth(newHealthValue, collisionPosX, collisionPosY);
        }
    }

    /// <summary>
    /// Set the current health of the player
    /// </summary>
    /// <param name="value"></param>
    public void SetHealth(int value, float collisionPosX = 0, float collisionPosY = 0)
    {
        EventManager.PlayerHealthChangeEvent e = new EventManager.PlayerHealthChangeEvent();

        int healthChange = value - currentHealth;

        currentHealth = value > baseHealth ? baseHealth : value;
        
        e.newCurrentHealth = currentHealth;
        e.healthChangeValue = healthChange;
        e.playerPosition = new Vector2(collisionPosX, collisionPosY);
        e.InvokeEvent();
    }


    public void ResetPlayer()
    {
        StopAllCoroutines();
        handlingOnHitInvulnerability = null;

        SetHealth(BaseHealth);
        playerSpriteRenderer.color = Color.white;
        SetPlayerSpriteDefault();
    }

    public void SetPlayerSpriteColor(Color color)
    {
        playerSpriteRenderer.color = color;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void PlayCollisionEffect(Vector2 collisionPoint)
    {
        collisionEffect.transform.position = collisionPoint;
        collisionEffect.Play();
    }

    public void SetPlayerSpriteDefault()
    {
        playerSpriteRenderer.sprite = playerDefaultSprite;
    }

    public void SetPlayerSpriteHappy()
    {
        playerSpriteRenderer.sprite = playerHappySprite;
    }
}
