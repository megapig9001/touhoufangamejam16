using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Range(0f, 30f)]
    [SerializeField] private float moveSpeed = 5;

    [Range(0f, 10f)]
    [SerializeField] private float fastMoveSpeedMultiplier = 1.5f;

    [Range(-360f, 360f)]
    [SerializeField] private float rotationSpeed  = 35;

    [SerializeField] private float knockbackTime = 0.2f;

    [SerializeField] private float knockbackForce = 40;
    [SerializeField] private float knockbackRotationForce = 30;

    private Rigidbody2D body;

    public bool PlayerInputDisabled { get; set; }
    public bool RotationDisabled { get; set; }

    public float DegreesPerSecond { get => rotationSpeed; set => rotationSpeed = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
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

        float angle = body.rotation + DegreesPerSecond * Time.fixedDeltaTime;
        body.MoveRotation(angle);

        if (Mathf.Abs(body.rotation) >= 360)
        {
            body.rotation = body.rotation % (360 * Mathf.Sign(body.rotation));
        }
    }

    private void ReverseRotation()
    {
        rotationSpeed *= -1;
    }

    private void HandleMovement()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(xInput, yInput);

        bool canMoveWithInput = !PlayerInputDisabled;

        if (canMoveWithInput)
        {
            float speed = Keyboard.current.zKey.isPressed ? MoveSpeed * fastMoveSpeedMultiplier : MoveSpeed;

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
        PlayerInputDisabled = true;
        RotationDisabled = true;

        body.linearVelocity = new Vector2(body.position.x - collision.GetContact(0).point.x, body.position.y - collision.GetContact(0).point.y).normalized
            * knockbackForce;
        
        body.angularVelocity = -1 * Mathf.Sign(rotationSpeed) * knockbackRotationForce;

        yield return new WaitForSeconds(knockbackTime);

        body.linearVelocity = Vector2.zero;
        PlayerInputDisabled = false;
        RotationDisabled = false;
    }
}
