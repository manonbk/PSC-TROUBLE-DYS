using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapesManager : MonoBehaviour
{

    public string[] shapeNames;
    int currentShapeIndex = -1;
    public DrawTemplate drawTemplate;
    public Draw draw;

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
        SetShapeIndex((currentShapeIndex+1)%shapeNames.Length);
    }

    public void ResetShapeIndex()
    {
        currentShapeIndex = -1;
        drawTemplate.EraseAll();
        draw.EraseAll();
    }

}
