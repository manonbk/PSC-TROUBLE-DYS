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
    void Start()
    {
        
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
