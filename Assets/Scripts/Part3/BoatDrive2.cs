using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BoatDrive2 : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 500.0f;
    public float maxRotationSpeed = 1.0f;

    public Transform[] checkpoints;
    public float distanceThreshold = 1.0f;
    private bool reached = false;

    private Rigidbody rb;

    public GameObject cameraObject;
    //public float maxAngleDeg;
    //float maxAngle;

    public LevelManager levelManager;

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
        //print(rb.velocity);
    }
    
    void Move()
    {
        float x = Mathf.Clamp(Input.acceleration.x,-2,2);
        float z = Mathf.Clamp(Input.acceleration.y, -2, 2);

        Vector3 forceDirection = new Vector3(x, 0, z);
        if (rb.velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Mathf.Min(rotationSpeed, maxRotationSpeed));
            transform.rotation = newRotation;
        }
        //transform.position += moveDirection * speed * Time.deltaTime;
        rb.AddForce(speed* forceDirection);

        // Check distance to arrival point
        float distance = Vector3.Distance(transform.position, checkpoints[levelManager.GetCurrentLevel()+1].position);
        if (distance <= distanceThreshold && !reached)
        {
            reached = true;
            // Normalement inutile désormais, remplacé par le compteur currentLevel

            //felicitations
            Debug.Log("Bravoo!");

            // On envoie le message au LevelManager
            levelManager.FinishLevel();
            
        }
    }

    public void Freeze(float t)
    {
        // Freezes for t seconds
        rb.isKinematic = true;
        StartCoroutine(WaitForSeconds(t));

    }

    IEnumerator WaitForSeconds(float t)
    {
        yield return new WaitForSeconds(t);
        rb.isKinematic = false;
    }

}
