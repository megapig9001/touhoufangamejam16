using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashController : MonoBehaviour
{
    [Range(0f, 30f)]
    public float dashSpeed = 12;

    [Range(0f, 7f)]
    public float dashAccelerationRate = 1.5f;

    public bool canDash = true;

    private PlayerController playerController;
    private Rigidbody2D body;

    private Coroutine handlingDash;

    private bool isDashing = false;

    private bool IsDashing { get => isDashing && handlingDash != null; }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Keyboard.current.xKey.isPressed)
        {
            if (!isDashing && !playerController.InHitstun)
            {
                isDashing = true;
                handlingDash = StartCoroutine(HandleDash());
            }
        }
    }

    private IEnumerator HandleDash()
    {
        Debug.Log("Dashing");
        playerController.RotationDisabled = true;
        playerController.PlayerInputDisabled = true;

        Vector2 dashDirection = CommonMethods.GetVectorFromAngle(playerController.CurrentRotationAngle);
        float time = 0;
        yield return new WaitWhile(() => {
            body.linearVelocity = Vector2.Lerp(Vector2.zero, dashDirection.normalized * dashSpeed, time * dashAccelerationRate);
            //body.linearVelocity = dashDirection.normalized * dashSpeed;
            time += Time.fixedDeltaTime;
            return Keyboard.current.xKey.isPressed || playerController.InHitstun;
        });

        //If the player exited out of the dash from entering hitstun, then these will get set in PlayerController once hitstun ends
        if (!playerController.InHitstun)
        {
            body.linearVelocity = Vector2.zero;
            playerController.RotationDisabled = false;
            playerController.PlayerInputDisabled = false;
        }
        Debug.Log("Finished dash");

        isDashing = false;
        handlingDash = null;
    }

    public void ResetPlayer()
    {
        StopAllCoroutines();
        isDashing = false;
        handlingDash = null;
        body.linearVelocity = Vector2.zero;
    }
}
