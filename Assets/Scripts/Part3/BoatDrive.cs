using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class BoatDrive : MonoBehaviour
{
    public float accelerationCoef;
    public float keyboardTurnCoef;
    public float maxAngleDeg;
    public float rotationCoef;
    float maxAngle;

    private Rigidbody rb;

    Gyroscope gyro;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gyro = Input.gyro;
        gyro.enabled = true;
        maxAngle = Mathf.PI / 180 * maxAngleDeg;
    }


    void FixedUpdate()
    {
        Accelerate();
        Turn();
        GyroAccelerate();
        print(rb.velocity);
        //rb.rotation = Quaternion.LookRotation(rb.position+ rb.velocity, Vector3.up); // Rotates towards velocity vector
    }

    void Accelerate ()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 forceToAdd = transform.forward;
            forceToAdd.y = 0;
            rb.AddForce(forceToAdd * accelerationCoef);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector3 forceToAdd = -transform.forward;
            forceToAdd.y = 0;
            rb.AddForce(forceToAdd * accelerationCoef);
        }

        /*
        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
        locVel = new Vector3(0, locVel.y, locVel.z);
        rb.velocity = new Vector3(transform.TransformDirection(locVel).x, rb.velocity.y, transform.TransformDirection(locVel).z);
        */
        // N'a plus de sens avec le gyroscope étant donné qu'on met à jour la rotation du bateau à partir de son vecteur vitesse
    }

    void Turn ()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(-Vector3.up * keyboardTurnCoef);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(Vector3.up * keyboardTurnCoef);
        }
    }

    void GyroAccelerate()
    {
        Vector3 forceDirection = new Vector3(-Mathf.Sin(Mathf.Clamp(gyro.attitude.x, -maxAngle, maxAngle)), 0, -Mathf.Sin(Mathf.Clamp(gyro.attitude.y, -maxAngle, maxAngle)));
        //forceDirection = Quaternion.AngleAxis(45,Vector3.up) * forceDirection;
        print(gyro.attitude);
        rb.AddForce(accelerationCoef*(1+Vector3.Dot(forceDirection.normalized,transform.forward))*forceDirection);
        rb.AddTorque(rotationCoef*Vector3.Cross(transform.forward, forceDirection)); // Gère la rotation du bateau
    }

    
}
