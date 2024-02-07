using System.Collections;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 4f; // vitesse deplacement de la camera
    // Start is called before the first frame update
    private TouchControls controls; //element d'un touch controls
    private Coroutine zoomCoroutine;
    private Transform cameraTransform;

    private void Awake(){
        controls = new TouchControls();    
        cameraTransform = Camera.main.transform; // Camera tagged as main
    }

    private void OnEnable(){
        controls.Enable();
    }

    private void OnDisable(){
        controls.Disable();
    }

    private void Start(){
        controls.Touch.SecondaryTouchContact.started += _ => ZoomStart();  // subscribing to an event without getting any parameter
        controls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }

    private void ZoomStart(){
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd(){
        StopCoroutine(zoomCoroutine);
    }

    IEnumerator ZoomDetection(){ // a coroutine: methode that can pause execution and return control to Unity, but then continnue where it left off on the following frame,
    //oftenn declared as an "IEnumerator" end with "yield"
    //started and ended by "StartCoroutine/EndCoroutine"
        float previousDistance = 0f, distance = 0f;//previous distance equals 0
        while (true){//always do it, until we lift the finger
            distance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(), controls.Touch.SecondaryFingerPosition.ReadValue<Vector2>());
            // Detection
            // Zoom out
            // Direction du mvt
            if (distance > previousDistance){
                Vector3 targetPosition = cameraTransform.position;
                targetPosition.z -= 1;
                cameraTransform.position = Vector3.Slerp(cameraTransform.position, targetPosition, Time.deltaTime * cameraSpeed); // Slerp(Vector3 a, Vector3 b, floatt t) inserer valeur t entre a et b
            }

            // Zoom in
            else if (distance < previousDistance){
                Vector3 targetPosition = cameraTransform.position;
                targetPosition.z += 1;
                //Camera.main.orthographicSize--; size of the amera, bigger value more zoomed, smaller less zoomed
                cameraTransform.position = Vector3.Slerp(cameraTransform.position, targetPosition, Time.deltaTime * cameraSpeed);
            
            }
            // Keep track of previous deistance for next loop
            previousDistance = distance; 
            yield return null; //make sure that it ends
        }
    }
}