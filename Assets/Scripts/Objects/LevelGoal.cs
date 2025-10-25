using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHead"))
        {
            new EventManager.PlayerReachGoalEvent().InvokeEvent();
        }

    }
}
