using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BoatDrive2 : MonoBehaviour
{
    public float speed = 15.0f;
    public float rotationSpeed = 90.0f;
    public float maxRotationSpeed = 20.0f;

    private Rigidbody rb;
    //public float maxAngleDeg;
    //float maxAngle;

    Gyroscope gyro;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gyro = Input.gyro;
        gyro.enabled = true;
        //maxAngle = Mathf.PI / 180 * maxAngleDeg;
    }

    void FixedUpdate()
    {
        Move();
        print(rb.velocity);
    }
    
    void Move()
    {
        float x = Input.acceleration.x;
        float z = Input.acceleration.y;

        Vector3 moveDirection = new Vector3(x, 0, z);
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Mathf.Min(rotationSpeed, maxRotationSpeed));
            transform.rotation = newRotation;
        }
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
