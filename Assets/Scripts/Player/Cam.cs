using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float sensX = 200f;
    public float sensY = 200f;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor for FPS
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -66f, 66f);

        // Apply rotation to camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
