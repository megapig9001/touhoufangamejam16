using EventManager;
using System;
using UnityEngine;

public class DashEnableItem : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Collider2D collider2d;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<PlayerDashController>().dashEnabled = true;
            JSAM.AudioManager.PlaySound(AudioLibrarySounds.GetItem);
            spriteRenderer.enabled = false;
            collider2d.enabled = false;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponentInParent<Collider2D>();
    }

    private void OnEnable()
    {
        LevelRestartEvent.AddListener(HandleLevelRestartEvent);
    }

    private void OnDisable()
    {
        LevelRestartEvent.RemoveListener(HandleLevelRestartEvent);
    }

    private void HandleLevelRestartEvent(LevelRestartEvent info)
    {
        spriteRenderer.enabled = true;
        collider2d.enabled = true;
    }
}
