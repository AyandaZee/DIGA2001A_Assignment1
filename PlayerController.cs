using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Fast Movement Settings")]
    public float walkSpeed = 9f;
    public float sprintSpeed = 16f;
    public float gravity = -9.81f;

    [Header("Look & Turning Settings")]
    public float mouseSensitivity = 3f;
    public Transform cameraTransform;

    [Header("Kick Settings")]
    public float softKickForce = 22f;
    public float hardKickForce = 45f;
    public float chipKickForce = 30f;

    [Header("References")]
    public Transform ballTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private float cameraPitch = 0f;
    private float yawRotation = 0f;

    // Kick timing trackers
    private float sPressTimer = 0f;
    private bool isHoldingS = false;
    private float lastSTapTime = 0f;

    private float speedMultiplier = 1f;
    private bool isFrozen = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        yawRotation = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isFrozen) return;

        HandleMouseLook();
        HandleMovementAndGravity();
        HandleKickingInput();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }

        yawRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, yawRotation, 0f);
    }

    private void HandleMovementAndGravity()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.A)) moveX -= 1f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;
        if (Input.GetKey(KeyCode.W)) moveZ += 1f;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.W);
        float currentSpeed = (isSprinting ? sprintSpeed : walkSpeed) * speedMultiplier;

        Vector3 moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMovement = (moveDirection * currentSpeed) + velocity;
        controller.Move(finalMovement * Time.deltaTime);
    }

    private void HandleKickingInput()
    {
        // --- 'S' KEY LOGIC (Soft Kick / Hold or Double Press for Hard Kick) ---
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Check for Double-Tap S
            if (Time.time - lastSTapTime < 0.35f)
            {
                ExecuteKick(hardKickForce, 0.25f);
                isHoldingS = false;
                lastSTapTime = 0f;
                return;
            }
            lastSTapTime = Time.time;

            sPressTimer = 0f;
            isHoldingS = true;
        }

        if (isHoldingS && Input.GetKey(KeyCode.S))
        {
            sPressTimer += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.S) && isHoldingS)
        {
            isHoldingS = false;
            if (sPressTimer >= 0.3f)
            {
                ExecuteKick(hardKickForce, 0.3f); // Held S = Hard Shot
            }
            else
            {
                ExecuteKick(softKickForce, 0.1f); // Quick S = Soft Pass
            }
        }

        // --- 'C' KEY LOGIC (Chip / Lob Shot) ---
        if (Input.GetKeyDown(KeyCode.C))
        {
            ExecuteKick(chipKickForce, 0.75f);
        }

        // --- SPACEBAR LOGIC (Instant Shot) ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteKick(hardKickForce, 0.35f);
        }
    }

    private void ExecuteKick(float force, float upwardAngle)
    {
        // Auto-locate ball if not assigned
        if (ballTransform == null)
        {
            GameObject foundBall = GameObject.FindWithTag("Ball");
            if (foundBall == null) foundBall = GameObject.Find("sharp.shooter ball");
            if (foundBall != null) ballTransform = foundBall.transform;
        }

        if (ballTransform == null) return;

        // Check if ball is anywhere near player (10 unit radius)
        float dist = Vector3.Distance(transform.position, ballTransform.position);
        if (dist <= 10f)
        {
            Rigidbody rb = ballTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Reset physics states completely
                rb.isKinematic = false;
                rb.WakeUp();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // Calculate kick direction straight forward where player/camera looks
                Vector3 aimDir = transform.forward;
                if (cameraTransform != null)
                {
                    aimDir = cameraTransform.forward;
                }

                Vector3 kickDirection = new Vector3(aimDir.x, upwardAngle, aimDir.z).normalized;

                // Apply realistic direct impulse shoot
                rb.AddForce(kickDirection * force, ForceMode.Impulse);

                Debug.Log("REALISTIC KICK EXECUTED! Force: " + force);
            }
        }
    }

    public void ApplyYellowCard(float duration)
    {
        StartCoroutine(YellowCardRoutine(duration));
    }

    private System.Collections.IEnumerator YellowCardRoutine(float duration)
    {
        speedMultiplier = 0.6f;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
    }

    public void ApplyRedCard(float duration)
    {
        StartCoroutine(RedCardRoutine(duration));
    }

    private System.Collections.IEnumerator RedCardRoutine(float duration)
    {
        isFrozen = true;
        yield return new WaitForSeconds(duration);
        isFrozen = false;
    }
}