using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BoatDrive2 : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 1.0f;

    public Transform[] checkpoints;
    public float distanceThreshold = 6.0f;

    private Rigidbody rb;

    public GameObject cameraObject;
    //public float maxAngleDeg;
    //float maxAngle;

    public LevelManager levelManager;

    Gyroscope gyro;

    public SaveData sd;

    bool savePosition = true;

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

        if (savePosition)
        {
            string posx = transform.position.x.ToString("0.00");
            string posz = transform.position.z.ToString("0.00");

            string acc_x = Input.acceleration.x.ToString("0.000");
            string acc_y = Input.acceleration.y.ToString("0.000");
            string acc_z = Input.acceleration.z.ToString("0.000");
            sd.add(string.Format("posBateauEtAccel;{0};{1};{2};{3};{4}", posx,posz, acc_x, acc_y, acc_z));

        }
        savePosition = !savePosition;
    }
    
    void Move()
    {
        float x = Mathf.Clamp(Input.acceleration.x,-2,2);
        float z = Mathf.Clamp(Input.acceleration.y, -2, 2);

        Vector3 forceDirection = new Vector3(x, 0, z);
        if (rb.velocity != Vector3.zero)
        {
            
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity);
            rb.AddTorque(-rotationSpeed*Vector3.Cross(forceDirection, transform.forward));
            //Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, rb.velocity.magnitude*Time.deltaTime * rotationSpeed);
            //transform.rotation = newRotation;
        }
        //transform.position += moveDirection * speed * Time.deltaTime;
        rb.AddForce(speed* forceDirection);

        // Check distance to arrival point
        float distance = Vector3.Distance(transform.position, checkpoints[levelManager.GetCurrentLevel()+1].position);
        if (distance <= distanceThreshold)
        {
            // On envoie le message au LevelManager
            levelManager.FinishLevel();

            savePosition = false;
            sd.add("finNiveau;" + levelManager.GetCurrentLevel().ToString());

        }
    }

    public void Freeze(float t)
    {
        // Freezes for t seconds
        rb.isKinematic = true;
        StartCoroutine(WaitForSeconds(t));

    }

    public void TeleportToCheckpoint(int i)
    {
        GetComponent<Transform>().position = checkpoints[i].position;
        GetComponent<Transform>().rotation = checkpoints[i].rotation;
    }

    IEnumerator WaitForSeconds(float t)
    {
        yield return new WaitForSeconds(t);
        rb.isKinematic = false;

        savePosition = true;
        sd.add("debutNiveau" + levelManager.GetCurrentLevel().ToString());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            sd.add("collision");
        }
    }

}
