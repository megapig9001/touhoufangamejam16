using DG.Tweening;
using EventManager;
using System;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    [SerializeField]
    GameObject goalItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHead"))
        {
            JSAM.AudioManager.PlaySound(AudioLibrarySounds.Clear);
            var goalEvent = new PlayerReachGoalEvent();
            goalEvent.goalPosition = transform.position;            
            goalEvent.InvokeEvent();

            if(goalItem != null)
            {
                goalItem.transform.DOScale(0, 1);
            }
        }

    }

    private void OnEnable()
    {
        LevelRestartEvent.AddListener(HandlePlayerDeathEvent);

    }


    private void OnDisable()
    {
        LevelRestartEvent.RemoveListener(HandlePlayerDeathEvent);
    }

    private void HandlePlayerDeathEvent(LevelRestartEvent info)
    {
        goalItem.transform.localScale = Vector3.one;
    }

}
