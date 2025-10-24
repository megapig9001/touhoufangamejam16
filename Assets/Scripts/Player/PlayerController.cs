using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerDashController))]
public class PlayerController : MonoBehaviour
{
    [Range(0f, 30f)]
    [Tooltip("The base move speed of the player.")]
    [SerializeField] private float moveSpeed = 5;
    public float StartingSpeed { get; private set; }

    [Range(0f, 10f)]
    [SerializeField] private float fastMoveSpeedMultiplier = 1.5f;

    [Range(0, 360f)]
    [Tooltip("The base rotation speed (clockwise) of the player.")]
    [SerializeField] private float rotationSpeed  = 35;
    public float StartingRotationSpeed { get; private set; }

    private bool flipRotation = false;

    [SerializeField] private float knockbackTime = 0.2f;

    [SerializeField] private float knockbackForce = 40;
    [SerializeField] private float knockbackRotationForce = 30;

    private Rigidbody2D body;

    public bool PlayerInputDisabled { get; set; }
    public bool RotationDisabled { get; set; }

    public float CurrentDegreesPerSecond { get => rotationSpeed * RotationDirection(); set => rotationSpeed = value; }
    public float CurrentMoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    public float CurrentRotationAngle { get => body.rotation; set => body.rotation = value; }

    public bool InHitstun { get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        StartingRotationSpeed = rotationSpeed;
        StartingSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void FixedUpdate()
    {
        HandleSpin();
    }

    private void HandleSpin()
    {
        if (RotationDisabled)
        {
            return;
        }

        float angle = body.rotation + CurrentDegreesPerSecond * Time.fixedDeltaTime;
        body.MoveRotation(angle);

        if (Mathf.Abs(body.rotation) >= 360)
        {
            body.rotation = body.rotation % (360 * Mathf.Sign(body.rotation));
        }
    }

    public void ReverseRotation()
    {
        flipRotation = flipRotation == false;
    }

    /// <summary>
    /// Returns -1 if the player is currently rotating counter-clockwise.
    /// </summary>
    /// <returns></returns>
    public int RotationDirection()
    {
        return flipRotation ? -1 : 1;
    }

    /// <summary>
    /// When parameters of the player (like speed, health, rotation, etc) need to be reset.
    /// Such as when the player respawns.
    /// </summary>
    public void ResetPlayer()
    {
        StopAllCoroutines();
        CurrentRotationAngle = 0;
        PlayerInputDisabled = false;
        RotationDisabled = false;
        flipRotation = false;
        moveSpeed = StartingSpeed;
        rotationSpeed = StartingRotationSpeed;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0;
        InHitstun = false;

        PlayerHealth healthController = GetComponent<PlayerHealth>();
        healthController.ResetPlayer();

        PlayerDashController dashController = GetComponent<PlayerDashController>();
        dashController.ResetPlayer();
    }

    private void HandleMovement()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(xInput, yInput);

        bool canMoveWithInput = !PlayerInputDisabled;

        if (canMoveWithInput)
        {
            float speed = Keyboard.current.zKey.isPressed ? CurrentMoveSpeed * fastMoveSpeedMultiplier : CurrentMoveSpeed;

            if (Mathf.Abs(xInput) > 0.001)
            {
                body.linearVelocity = new Vector2(xInput * speed, body.linearVelocity.y);
            }

            if (Mathf.Abs(yInput) > 0.001)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, yInput * speed);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        StartCoroutine(HandleHitstun(collision));
    }

    private IEnumerator HandleHitstun(Collision2D collision)
    {
        //Hitstun has already been triggered in the past knockbackTime-seconds. Just reapply the velocities and nothing else.
        if (!InHitstun)
        {
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0;
            InHitstun = true;
            PlayerInputDisabled = true;
            RotationDisabled = true;

            body.linearVelocity = new Vector2(body.position.x - collision.GetContact(0).point.x, body.position.y - collision.GetContact(0).point.y).normalized
                * knockbackForce;

            body.angularVelocity = -1 * Mathf.Sign(rotationSpeed) * knockbackRotationForce;

            yield return new WaitForSeconds(knockbackTime);

            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0;
            PlayerInputDisabled = false;
            RotationDisabled = false;
            InHitstun = false;
        }
        else
        {
            body.linearVelocity = new Vector2(body.position.x - collision.GetContact(0).point.x, body.position.y - collision.GetContact(0).point.y).normalized
                * knockbackForce;

            body.angularVelocity = -1 * Mathf.Sign(rotationSpeed) * knockbackRotationForce;
            yield return null;
        }
    }
}
