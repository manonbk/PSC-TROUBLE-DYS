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
    public float distanceThreshold = 6.0f;

    private Rigidbody rb;

    //public GameObject cameraObject;
    //public float maxAngleDeg;
    //float maxAngle;

    public LevelManager levelManager;

    Gyroscope gyro;

    //pour enregistrer les données
    public SaveData sd ;
    private bool savePosition = true; // 1 on sauvegarde la position, 0 on ne sauvegarde pas
    private string posx;
    private string posy;
    private string posz;

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
        if( savePosition){
            posx = transform.position.x.ToString("0.000");
            posy = transform.position.y.ToString("0.000");
            posz = transform.position.z.ToString("0.000");
            sd.add("positionBateau,"+ posx+ "," + posy + "," + posz);
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
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Mathf.Min(rotationSpeed, maxRotationSpeed));
            transform.rotation = newRotation;
        }
        //transform.position += moveDirection * speed * Time.deltaTime;
        rb.AddForce(speed* forceDirection);

        // Check distance to arrival point
        float distance = Vector3.Distance(transform.position, checkpoints[levelManager.GetCurrentLevel()+1].position);
        if (distance <= distanceThreshold)
        {

            // felicitations
            Debug.Log("Bravoo!");

            // On ajoute les données du niveau dans le fichier
            savePosition = false;
            sd.add("finNiveau," + levelManager.GetCurrentLevel().ToString());

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

    public void TeleportToCheckpoint(int i)
    {
        GetComponent<Transform>().position = checkpoints[i].position;
        GetComponent<Transform>().rotation = checkpoints[i].rotation;
    }

    IEnumerator WaitForSeconds(float t)
    {
        yield return new WaitForSeconds(t);
        rb.isKinematic = false;

        // debut du niveau suivant
        savePosition = true;
    }

}
