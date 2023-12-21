using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShapesManager : MonoBehaviour
{

    public string[] shapeNames;
    int currentShapeIndex = -1;
    public DrawTemplate drawTemplate;
    public Draw draw;
    public GameObject startButton;
    public GameObject endPanel;

    // Start is called before the first frame update

    private void Start()
    {
        Invoke("DelayedStart", .5f);
    }
    void DelayedStart()
    {
        string shapeName = shapeNames[0];
        drawTemplate.SetShape(shapeName);
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
