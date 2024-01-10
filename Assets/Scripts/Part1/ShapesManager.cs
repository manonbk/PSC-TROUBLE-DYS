using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShapesManager : MonoBehaviour
{

    // Script qui gère les images : fonctions pour dessiner, 

    public string[] shapeNames;
    int currentShapeIndex = -1;
    public DrawTemplate drawTemplate; // C'est un script
    public Draw draw;
    public GameObject startButton;
    public GameObject endPanel;

    // Start is called before the first frame update

    private void Start()
    {
        Invoke("DelayedStart", .5f); // Executes the specified delegate on the thread that owns the control's underlying window handle. ?? je sais pas trop
    }

    void DelayedStart()
    {
        string shapeName = shapeNames[0];
        drawTemplate.SetShape(shapeName); // appelle la fonction qui dessine la forme demandée (va chercher dans le script drawTemplate la fonction SetShape)
        draw.SetShape(shapeName);
    }
 

    public void StartGame()
    {
        draw.hasStarted = true;
        startButton.SetActive(false);
        if (currentShapeIndex == -1)
        {
            NextShape();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetShapeIndex(int i)
    {
        currentShapeIndex = i;
        string shapeName = shapeNames[currentShapeIndex];
        drawTemplate.SetShape(shapeName);
        draw.SetShape(shapeName);
    }

    public void NextShape()
    {
        if (currentShapeIndex == shapeNames.Length - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        SetShapeIndex(currentShapeIndex+1);
    }

    public void PreviousShape()
    {
        if (currentShapeIndex > 0)
        SetShapeIndex(currentShapeIndex-1);
    }

    public void ResetShapeIndex()
    {
        currentShapeIndex = -1;
        drawTemplate.EraseAll();
        draw.EraseAll();
    }

    public void OnMainButtonPressed()
    {
        if (draw.isDrawingFinished)
        {
            NextShape();
        }
        else
        {
            draw.ConvertToImage();
        }
    }
}
