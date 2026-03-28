using UnityEngine;
using UnityEngine.InputSystem; // Add this line!

public class EditorCameraLook : MonoBehaviour
{
    public float sensitivity = 0.1f;
    private float rotationX = 0;
    private float rotationY = 0;

    void Update()
    {
        // New Input System check for the Left Alt key
        if (Keyboard.current.leftAltKey.isPressed)
        {
            // New Input System way to get Mouse Delta
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            rotationX += mouseDelta.x * sensitivity;
            rotationY -= mouseDelta.y * sensitivity;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
        }
    }
}