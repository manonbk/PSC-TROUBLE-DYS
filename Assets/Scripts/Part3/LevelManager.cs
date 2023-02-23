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

        boatDrive.TeleportToCheckpoint(currentLevel);
        // On lance EN MEME TEMPS les actions suivantes (en parall�le) :

        cameraMovement.GoToCheckpoint(currentLevel,2);// D�placement de la cam�ra sur 2 sec
        if( currentLevel == 2){
            print("niveau 2");
        }
        whaleMovement.GoToCheckpoint(currentLevel+1, 4, 2);// La baleine attend 2 secondes, puis bouge en 4 secondes
        boatDrive.Freeze(4);// Le bateau est gel� pendant 4 secondes
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
