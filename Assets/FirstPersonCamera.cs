using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    float yaw = 0.0f, pitch = 0.0f;
    [SerializeField] float sensitivity = 2.0f;
    private Rigidbody rb;
    public Transform Camera;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    
    void Update()
    {
         Look();
    }

    void Look()
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);
        yaw += Input.GetAxisRaw("Mouse X") * sensitivity;
        Camera.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }
}