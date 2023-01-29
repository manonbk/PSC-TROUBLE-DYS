using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    int currentLevel;
    public SmoothMove cameraMovement;
    public WhaleMovement whaleMovement;
    public BoatDrive2 boatDrive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishLevel()
    {
        currentLevel++;

        // On lance EN MEME TEMPS les actions suivantes (en parallèle) :

        cameraMovement.GoToCheckpoint(currentLevel,2);// Déplacement de la caméra sur 2 sec
        whaleMovement.GoToCheckpoint(currentLevel+1, 4, 2);// La baleine attend 2 secondes, puis bouge en 4 secondes
        boatDrive.Freeze(4);// Le bateau est gelé pendant 4 secondes
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
