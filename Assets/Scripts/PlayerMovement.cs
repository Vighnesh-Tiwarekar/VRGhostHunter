using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3.0f;
    public float rotationSpeed = 100.0f;

    [Header("References")]
    public Transform vrCamera; // Assign 'Main Camera' here

    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Transform cameraOffset;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("PlayerMovement: CharacterController not found on this GameObject!");
            enabled = false;
            return;
        }

        // Ensure the camera reference is set
        if (vrCamera == null)
        {
            if (Camera.main != null)
                vrCamera = Camera.main.transform;
            else
            {
                Debug.LogError("PlayerMovement: No VR Camera assigned and no Main Camera found!");
                enabled = false;
                return;
            }
        }

        // Fix for 3 DOF VR head height (e.g. Cardboard)
        // Ensure the camera is wrapped in an offset so we can raise it from the floor
        if (vrCamera.parent == transform)
        {
            GameObject offsetObj = new GameObject("CameraOffset");
            cameraOffset = offsetObj.transform;
            cameraOffset.SetParent(transform);
            cameraOffset.localPosition = new Vector3(0, 1.6f, 0); // Average eye height
            cameraOffset.localRotation = Quaternion.identity;
            vrCamera.SetParent(cameraOffset);
            vrCamera.localPosition = Vector3.zero;
        }
    }

    // Input System Messages
    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => lookInput = value.Get<Vector2>();

    void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    void HandleRotation()
    {
        if (vrCamera == null)
            return;
            
        // This rotates the PLAYER BODY (the parent).
        // The Camera (the head) rotates independently via the Gyro/TrackedPoseDriver.
        float turn = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);
    }

    void HandleMovement()
    {
        if (vrCamera == null || characterController == null)
            return;
            
        // 1. Get Camera Direction
        Vector3 forward = vrCamera.forward;
        Vector3 right = vrCamera.right;

        // 2. Kill the Y-axis so you don't walk into the air or floor
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // 3. Calculate direction based on where the player is LOOKING
        Vector3 moveDirection = (forward * moveInput.y) + (right * moveInput.x);

        // 4. SimpleMove handles Gravity automatically (useful for stairs in the house!)
        characterController.SimpleMove(moveDirection * moveSpeed);
    }
}