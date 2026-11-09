using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4.2f;
    public float gravity = -18f;
    public float jumpHeight = 1.0f;
    [Header("Look")]
    public Transform cameraRoot;
    public float lookSensitivity = 2.0f;
    public float mobileLookSensitivity = 0.12f;
    public float maxLookAngle = 82f;

    CharacterController controller;
    float verticalVelocity;
    float pitch;
    Vector2 mobileLook;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cameraRoot == null && Camera.main != null) cameraRoot = Camera.main.transform;
    }

    void Update()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 touchMove = MobileInput.Move;
        if (touchMove.sqrMagnitude > move.sqrMagnitude) move = touchMove;
        move = Vector2.ClampMagnitude(move, 1f);

        Vector3 direction = transform.right * move.x + transform.forward * move.y;
        controller.Move(direction * walkSpeed * Time.deltaTime);

        if (controller.isGrounded && verticalVelocity < 0) verticalVelocity = -2f;
        if (controller.isGrounded && (Input.GetKeyDown(KeyCode.Space) || MobileInput.JumpPressed))
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * lookSensitivity;
        Vector2 look = mouse + MobileInput.ConsumeLook() * mobileLookSensitivity;
        transform.Rotate(Vector3.up * look.x);
        pitch = Mathf.Clamp(pitch - look.y, -maxLookAngle, maxLookAngle);
        if (cameraRoot != null) cameraRoot.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
