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

        cameraMovement.GoToCheckpoint(currentLevel,2);
        boatDrive.Freeze(2);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
