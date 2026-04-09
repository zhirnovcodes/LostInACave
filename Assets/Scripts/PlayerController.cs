using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 5f;

    [Header("Mouse Look")]
    public Transform Head;
    public Transform Arm;
    public float MouseSensitivity = 2f;

    [Header("Rotation Limits  x = min  y = max")]
    public Vector2 PitchLimit = new Vector2(-80f, 80f);
    public Vector2 YawLimit   = new Vector2(-360f, 360f);
    public Vector2 RollLimit  = new Vector2(0f, 0f);

    private Rigidbody Rb;
    private float Pitch = 0f;
    private float Yaw   = 0f;
    private float Roll  = 0f;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Rb.freezeRotation = true;

        if (Head == null)
        {
            Head = transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Yaw = transform.eulerAngles.y;
        Pitch = Head.localEulerAngles.x;
        if (Pitch > 180f)
        {
            Pitch -= 360f;
        }
    }

    private void Update()
    {
        HandleMouseLook();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        var kb = Keyboard.current;
        if (kb == null)
        {
            return;
        }

        float h = (kb.dKey.isPressed ? 1f : 0f) - (kb.aKey.isPressed ? 1f : 0f);
        float v = (kb.wKey.isPressed ? 1f : 0f) - (kb.sKey.isPressed ? 1f : 0f);

        Vector3 moveDir = transform.right * h + transform.forward * v;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        Vector3 targetVelocity = moveDir * MoveSpeed;
        targetVelocity.y       = Rb.linearVelocity.y;
        Rb.linearVelocity      = targetVelocity;
    }

    private void HandleMouseLook()
    {
        var mouse = Mouse.current;
        if (mouse == null)
        {
            return;
        }

        Vector2 delta = mouse.delta.ReadValue();
        float mouseX  = delta.x * MouseSensitivity * 0.1f;
        float mouseY  = delta.y * MouseSensitivity * 0.1f;

        Yaw   = Mathf.Clamp(Yaw + mouseX, YawLimit.x, YawLimit.y);
        Pitch = Mathf.Clamp(Pitch - mouseY, PitchLimit.x, PitchLimit.y);
        Roll  = Mathf.Clamp(Roll, RollLimit.x, RollLimit.y);

        Rb.MoveRotation(Quaternion.Euler(0f, Yaw, 0f));
        Head.localRotation = Quaternion.Euler(Pitch, 0f, Roll);
        Arm.localRotation = Quaternion.Euler(Pitch, 0f, Roll);
    }
}
