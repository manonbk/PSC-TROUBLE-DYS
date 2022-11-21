using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DrawTemplate : MonoBehaviour
{
    private LineRenderer baseLine;

    // Start is called before the first frame update
    void Start()
    {
        baseLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShape(string shapeName)
    {
        // On récupère les données du fichier JSON correspondant au shapeName
        var textFile = Resources.Load<TextAsset>("Shapes/" + shapeName);
        Shape data = new();
        if (textFile != null)
        {
            string jsonString = textFile.text;
            data = JsonUtility.FromJson<Shape>(jsonString);
            DrawBaseShape(data.points);
        }
        else
        {
            print("Erreur lors de la lecture du fichier : " + "Shapes/" + shapeName + ".json");
        }

    }

    public void DrawBaseShape(float[] shapePoints)
    {
        Rect rect = GetComponent<RectTransform>().rect;
        int nPoints = shapePoints.Length / 2;

        float xmin = rect.min.x;
        float ymin = rect.min.y;
        float xmax = rect.max.x;
        float ymax = rect.max.y;

        float scale = Mathf.Min(xmax - xmin, ymax - ymin) / 2;

        Vector3[] points = new Vector3[nPoints];
        for (int i = 0; i < nPoints; i++)
        {
            points[i].x = scale * shapePoints[2 * i];
            points[i].y = scale * shapePoints[2 * i + 1];
        }
        baseLine.positionCount = nPoints;
        baseLine.SetPositions(points);

    }

    public void EraseAll()
    {
        baseLine.positionCount = 0;
    }
}
