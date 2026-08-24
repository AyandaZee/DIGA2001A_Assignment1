using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public float mouseSensitivity = 0.1f;
    public Transform cameraTransform;

    [Header("Kick Settings")]
    public float kickReachDistance = 3.5f;
    public float kickForce = 18f;

    private CharacterController controller;
    private Vector3 velocity;
    private float cameraPitch = 0f;

    private float speedMultiplier = 1f;
    private bool isFrozen = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isFrozen) return;

        // Direct Mouse Look
        if (Keyboard.current != null && Mouse.current != null)
        {
            Vector2 lookInput = Mouse.current.delta.ReadValue();
            float mouseX = lookInput.x * mouseSensitivity;
            float mouseY = lookInput.y * mouseSensitivity;

            cameraPitch -= mouseY;
            cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);
            if (cameraTransform != null)
            {
                cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
            }
            transform.Rotate(Vector3.up * mouseX);

            // WASD Keyboard Input
            float moveX = 0f;
            float moveZ = 0f;

            if (Keyboard.current.wKey.isPressed) moveZ += 1f;
            if (Keyboard.current.sKey.isPressed) moveZ -= 1f;
            if (Keyboard.current.aKey.isPressed) moveX -= 1f;
            if (Keyboard.current.dKey.isPressed) moveX += 1f;

            bool isSprinting = Keyboard.current.leftShiftKey.isPressed;
            float currentSpeed = (isSprinting ? sprintSpeed : walkSpeed) * speedMultiplier;

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move.normalized * currentSpeed * Time.deltaTime);

            // Kicking Logic (Press E)
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                TryKickBall();
            }
        }

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void TryKickBall()
    {
        GameObject ball = GameObject.Find("Football");
        if (ball != null)
        {
            float dist = Vector3.Distance(transform.position, ball.transform.position);
            if (dist <= kickReachDistance)
            {
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    Vector3 kickDir = (ball.transform.position - transform.position).normalized;
                    kickDir.y = 0.35f; // Upward arc
                    rb.AddForce(kickDir * kickForce, ForceMode.Impulse);
                    Debug.Log("BALL KICKED!");
                }
            }
        }
    }

    public void ApplyYellowCard(float duration)
    {
        StartCoroutine(YellowCardRoutine(duration));
    }

    private IEnumerator YellowCardRoutine(float duration)
    {
        speedMultiplier = 0.5f;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
    }

    public void ApplyRedCard(float duration)
    {
        StartCoroutine(RedCardRoutine(duration));
    }

    private IEnumerator RedCardRoutine(float duration)
    {
        isFrozen = true;
        yield return new WaitForSeconds(duration);
        isFrozen = false;
    }
}