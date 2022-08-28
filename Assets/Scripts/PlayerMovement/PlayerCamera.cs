using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Mouse")]
    public float SensitivityX;
    public float SensitivityY;

    [Header("Camera")]
    public Transform Orientation;

    [HideInInspector] public float RotationX;
    [HideInInspector] public float RotationY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SensitivityY;

        RotationX -= mouseY;
        RotationY += mouseX;

        RotationX = Mathf.Clamp(RotationX, -90f, 90f);

        // Rotation camera and orientation
        transform.rotation = Quaternion.Euler(RotationX, RotationY, 0);
        Orientation.rotation = Quaternion.Euler(0, RotationY, 0);
    }
}
