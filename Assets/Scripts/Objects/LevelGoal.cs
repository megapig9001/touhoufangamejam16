using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHead"))
        {
            JSAM.AudioManager.PlaySound(AudioLibrarySounds.Clear);
            var goalEvent = new EventManager.PlayerReachGoalEvent();
            goalEvent.goalPosition = transform.position;            
            goalEvent.InvokeEvent();
        }

    }
}
