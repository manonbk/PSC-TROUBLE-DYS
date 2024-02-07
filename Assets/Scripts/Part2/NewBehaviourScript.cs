using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public LayerMask interactableObjects; // seuls les objects dans cette layer peuvent etre bouges
    public float minScale = .3f; // limites pour le scaling
    public float maxScale = 3f;

    private GameObject correctForm; //cible
    public float distance; //distance d'erreur autorisee
    public float taille; //taille d'erreur autorisee
    public float rotation; //rotation d'erreur autorisee
    private bool correctPosition;
    private bool correctRotation;
    private bool correctScale;
 
    private Transform selectedTransform;
    private GameObject selectedObject;
 
    private Touch firstTouch;
    private Vector3 firstTouchPosition;
    private Vector3 firstTouchOffset;
 
    private Touch secondTouch;
    private Vector3 secondTouchPosition;
 
    private float initialDistance;
    private float currentDistance;
    private Vector3 initialScale;
 
    private Vector3 lastDirection;
    private Vector3 currentDirection;

    //MESURES
    
    private int nbLeverDoigt; //compte le nombre de lever de doigt dans un niveau
    List<int> LeverDoigt = new List<int>();
    List<float> distanceCible = new List<float>();
    List<float> rotationCible = new List<float>();
    List<float> tailleCible = new List<float>();
    
    private float timeGeneral;

    public float cteangle;
    //Calcul du score
    public float normDistance;
    public float normRotation;
    public float normTaille;
    public float normLeverDoigt;
    public float normTime;
 
    private void Update() {
        HandleTouches();
        timeGeneral += Time.deltaTime;
    }
 
    private void HandleTouches() {
        if(Input.touchCount > 0) {
            HandleFirstTouch();
            if(Input.touchCount > 1) {
                HandleSecondTouch();
            }
        }
    }
 
    private void HandleFirstTouch() {
        // get the first touch's info and world-position
        firstTouch = Input.GetTouch(0);
        firstTouchPosition = Camera.main.ScreenToWorldPoint(firstTouch.position);
 
        // if there's no object selected
        if(selectedTransform == null) {
            // check if a touch just began
            if(firstTouch.phase == TouchPhase.Began) {
                // see what is under the touch
                Collider2D hitCollider = Physics2D.OverlapPoint(firstTouchPosition, interactableObjects);
                if(hitCollider != null) {
                    // if an object was hit, save it and the distance between touch and object center
                    selectedObject = hitCollider.gameObject;
                    selectedTransform = hitCollider.transform;
                    firstTouchOffset = selectedTransform.position - firstTouchPosition;
                    timeForme = 0;
                    correctForm = GameObject.FindWithTag(selectedTransform.name+"cible");
                }
            }
        } else {
            // if there's already an object selected, see if the touch has moved or ended
            switch(firstTouch.phase) {
                case TouchPhase.Moved:
                    // if the touch moved, have the object follow if there are no other touches
                    if(Input.touchCount == 1) {
                        SetPosition(firstTouchPosition);
                    }
                    break;
                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    nbLeverDoigt +=1;
                    if (correctPosition && correctRotation && correctScale){
                        //Enregistrement des mesures pour les formes qui s'aimantent
                        distanceCible.Add(Mathf.Abs(selectedTransform.position.magnitude-correctForm.transform.position.magnitude));
                        rotationCible.Add(Mathf.Abs(Mathf.Abs(selectedTransform.localRotation.eulerAngles.z - correctForm.transform.localRotation.eulerAngles.z)));
                        tailleCible.Add(Mathf.Abs(selectedTransform.localScale.magnitude - correctForm.transform.localScale.magnitude));
                        LeverDoigt.Add(nbLeverDoigt);
                        nbLeverDoigt=0;
                        

                        //Aimantation
                        Debug.Log(selectedObject.name);
                        selectedTransform.position=correctForm.transform.position;
                        Quaternion correctFormRotation = correctForm.transform.rotation;
                        selectedTransform.rotation = correctFormRotation;
                        selectedTransform.localScale = new Vector3(correctForm.transform.localScale.x, correctForm.transform.localScale.y, correctForm.transform.localScale.z);
                        //remove object from Layer interactableObjects
                        int layerContour = LayerMask.NameToLayer("Contour");
                        selectedObject.layer = layerContour;
                        //change Tag
                        selectedObject.tag = "Completed";
                    }
                    
                    // deselect the object if the touch is lifted
                    selectedTransform = null;
                    break;
            }
        }
    }
 
    private void HandleSecondTouch() {
        // if there is currently a selected object
        if(selectedTransform != null) {
            // get the touch info and world position
            secondTouch = Input.GetTouch(1);
            secondTouchPosition = Camera.main.ScreenToWorldPoint(secondTouch.position);
 
            // if the second touch just began
            if(secondTouch.phase == TouchPhase.Began) {
                // save the direction between first and second touch
                currentDirection = secondTouchPosition - firstTouchPosition;
                // initialize the 'last' direction for comparisons
                lastDirection = currentDirection;
 
                // get the distance between the touches, saving the initial distance for comparison
                currentDistance = (lastDirection).sqrMagnitude;
                initialDistance = currentDistance;
 
                // save the object's starting scale
                initialScale = selectedTransform.localScale;
            } else if(secondTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Moved) {
                //
                // if either touch moved, update the rotation and scale
                //
           
                // get the current direction between the touches
                currentDirection = secondTouchPosition - firstTouchPosition;
 
                // find the angle difference between the last direction and the current
                float angle = Vector3.Angle(currentDirection, lastDirection);
 
                // Vector3.Angle only outputs positives, so check if it should be a negative angle
                Vector3 cross = Vector3.Cross(currentDirection, lastDirection);
                if(cross.z > 0) {
                    angle = -angle;
                }
 
                // update rotation
                SetRotation(angle);
                // save this direction for next frame's comparison
                lastDirection = currentDirection;
 
                // get the current distance between touches
                currentDistance = (currentDirection).sqrMagnitude;
                // get what % of the intial distance this new distance is
                float difference = currentDistance / initialDistance;
                // scale by that percentage
                SetScale(difference);
            }
 
            // if the second touch ended
            if(secondTouch.phase == TouchPhase.Ended || secondTouch.phase == TouchPhase.Canceled) {
                // update the first touch offset so dragging will start again from wherever the first touch is now
                firstTouchOffset = selectedTransform.position - firstTouchPosition;
            }
        }
    }
 
    // update object position without changing Z
    private void SetPosition(Vector3 position) {
        if(selectedTransform != null) {
            correctPosition=false;
            Vector3 newPosition = position + firstTouchOffset;
            newPosition.z = selectedTransform.position.z;
            selectedTransform.position = newPosition;
            Debug.Log(correctForm.name);
            if (Mathf.Abs(selectedTransform.position.x - correctForm.transform.position.x) <= distance &&  //distance a redefinir = precision qu'on attend pour lacher
            Mathf.Abs(selectedTransform.position.y - correctForm.transform.position.y) <= distance) {
                correctPosition=true;
            }
        }

    }
 
    // rotate the object by the angle about the Z axis
    private void SetRotation(float angle) {
        if(selectedTransform != null) {
            correctRotation=false;
            selectedTransform.Rotate(Vector3.forward, angle*cteangle);
            if(Mathf.Abs(selectedTransform.localRotation.eulerAngles.z - correctForm.transform.localRotation.eulerAngles.z) <= rotation){
                correctRotation=true;
            }
        }
    }
 
    // scale the object by the percentage difference
    // taking into account min/max scale value
    private void SetScale(float percentDifference) {
        if(selectedTransform != null) {
            correctScale=false;
            Vector3 newScale = initialScale * percentDifference;
            if(newScale.x > minScale && newScale.y > minScale && newScale.x < maxScale && newScale.y < maxScale) {
                newScale.z = 1f;
                selectedTransform.localScale = newScale;
            }
            if (Mathf.Abs(selectedTransform.localScale.magnitude - correctForm.transform.localScale.magnitude) <= taille) {
                correctScale=true;
            }  //check de la taille

        }
    }

    public void switchScene(){
        float time = timeGeneral;
        //Enregistrement des mesures pour les objets restants
        GameObject[] list;
        bool formesManquantes = false;
        list = GameObject.FindGameObjectsWithTag("Remaining");
        if (list!=null){
            formesManquantes=true;
            foreach (GameObject go in list){
                correctForm = GameObject.FindWithTag(go.transform.name+"cible");
                distanceCible.Add(Mathf.Abs(go.transform.position.magnitude-correctForm.transform.position.magnitude));
                rotationCible.Add(Mathf.Abs(Mathf.Abs(go.transform.localRotation.eulerAngles.z - correctForm.transform.localRotation.eulerAngles.z)));
                tailleCible.Add(Mathf.Abs(go.transform.localScale.magnitude - correctForm.transform.localScale.magnitude));          
            }
        }

        //Calcul du score
        double Score = getScore(formesManquantes, time);
        Debug.Log(Score);
        
        //Changement de scene
        if (Score<0.7) {
            //si deux niveaux un
            if (SceneManager.GetActiveScene().buildIndex == 3) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            }
            if (SceneManager.GetActiveScene().buildIndex == 4 ||SceneManager.GetActiveScene().buildIndex == 5 ) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
            }
            if (SceneManager.GetActiveScene().buildIndex == 6) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -2);
            }
        }
        else {
            //si deux niveaux deux
            if (SceneManager.GetActiveScene().buildIndex == 3) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
            }
            if (SceneManager.GetActiveScene().buildIndex == 4) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            }
            if (SceneManager.GetActiveScene().buildIndex == 5) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            }
            if (SceneManager.GetActiveScene().buildIndex == 6) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
            }
        }
    }

    private double getScore(bool formesManquantes, float time){
        double score;
        if (formesManquantes){
            score = 0;
            return score;
        }

        //chercher les trucs dans les tableaux et ecrire la formule
        int length = distanceCible.Count;
        double sommeMesure = 0;
        double sommeFormes = 0;
        for (int i =0; i<length; i++){
            double distanceT = System.Math.Tanh(distanceCible[i]/normDistance);
            double tailleT = System.Math.Tanh(tailleCible[i]/normTaille);
            double rotationT = System.Math.Tanh(rotationCible[i]/normTaille);
            double leverDoigtT = System.Math.Tanh(LeverDoigt[i]/normLeverDoigt);
            sommeMesure += distanceT + tailleT + rotationT + leverDoigtT;
        }
        sommeFormes = sommeMesure/(length+1); //sommeMesure/nbFormes
        sommeFormes += System.Math.Tanh(time/normTime);
        
        score = (5 - sommeFormes)/5;
        return score;
    }
}
