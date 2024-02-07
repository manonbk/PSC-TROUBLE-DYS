using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scale : MonoBehaviour
{
    private float distanceInitialePincement;
    private Vector3 echelleInitiale;

    void Update()
    {
        // Vérifie si deux touches sont actives sur l'écran
        if (Input.touchCount == 2)
        {
            // Récupère les informations de la première et de la deuxième touche
            Touch touche1 = Input.GetTouch(0);
            Touch touche2 = Input.GetTouch(1);

            // Si l'une des touches commence, enregistre la distance initiale de pincement et l'échelle initiale de l'objet
            //if (touche1.phase == TouchPhase.Began || touche2.phase == TouchPhase.Began)
            //{
                // Vérifie si les touches touchent l'objet
                if (TouchesSurObjet(touche1.position) && TouchesSurObjet(touche2.position))
                {
                    distanceInitialePincement = Vector2.Distance(touche1.position, touche2.position);
                    echelleInitiale = transform.localScale;
                }
                else{
                    return;
                } 
            //}
            // Si l'une des touches est déplacée, effectue le zoom
            if (touche1.phase == TouchPhase.Moved || touche2.phase == TouchPhase.Moved)
            {
                    // Calcule la distance actuelle de pincement entre les deux touches
                    float distancePincementActuelle = Vector2.Distance(touche1.position, touche2.position);

                    // Calcule la différence de pincement par rapport à la distance initiale
                    float differencePincement = distancePincementActuelle - distanceInitialePincement;

                    // Ajuste le facteur d'échelle en fonction de la différence de pincement
                    float facteurEchelle = 0.01f;
                    Vector3 echelle = echelleInitiale + new Vector3(differencePincement * facteurEchelle, differencePincement * facteurEchelle, differencePincement * facteurEchelle);

                    // Limite l'échelle pour éviter que l'objet ne devienne trop petit ou trop grand
                    echelle.x = Mathf.Clamp(echelle.x, 0.1f, 10f);
                    echelle.y = Mathf.Clamp(echelle.y, 0.1f, 10f);

                    // Applique la nouvelle échelle à l'objet
                    transform.localScale = echelle;
                }
            }

    // Vérifie si une position de touche est sur ou à proximité de l'objet
    bool TouchesSurObjet(Vector2 positionTouche)
    {
        Ray rayon = Camera.main.ScreenPointToRay(positionTouche);
        RaycastHit2D toucheHit;

        // Définir ici le masque de collision approprié pour l'objet (peut nécessiter un ajustement selon la scène)
        int masqueCollision = 1 << LayerMask.NameToLayer("VotreCouche");

        toucheHit = Physics2D.GetRayIntersection(rayon, Mathf.Infinity, masqueCollision);

        return toucheHit.collider != null && toucheHit.collider.gameObject == gameObject;
    }
}
}