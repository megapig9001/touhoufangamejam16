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

    [Range(0f, 7f)]
    [SerializeField] private float dashCooldown = 0.5f;

    [SerializeField] bool enableDashByDefault = false;

    public bool dashEnabled { get; set; }

    private PlayerController playerController;
    private Rigidbody2D body;

    private Coroutine handlingDash;

    private bool isDashing = false;

    private bool IsDashing { get => isDashing && handlingDash != null; }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        body = GetComponent<Rigidbody2D>();
        dashEnabled = enableDashByDefault;
    }

    private void Update()
    {
        if (dashEnabled && Keyboard.current.xKey.isPressed)
        {
            if (handlingDash == null && !playerController.InHitstun)
            {
                handlingDash = StartCoroutine(HandleDash());
            }
        }
    }

    private IEnumerator HandleDash()
    {
        isDashing = true;
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

        isDashing = false;

        Debug.Log("waiting for cooldown: " + dashCooldown);
        yield return new WaitForSeconds(dashCooldown);

        handlingDash = null;
    }

    public void ResetPlayer()
    {
        StopAllCoroutines();
        isDashing = false;
        handlingDash = null;
        body.linearVelocity = Vector2.zero;
        dashEnabled = enableDashByDefault;
    }
}
