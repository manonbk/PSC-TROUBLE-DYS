using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape
{
    public string name;
    public float[] points;
    public float length;
}

public class Draw : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Coroutine drawing;
    private bool mouse_over = false;
    private Vector3[] points; // Points de la forme (coordonnées WorldGUI relatives à l'élément de GUI)
    private Vector2[] screenpoints; // Idem (coordonnées absolues; Vector2 suffit ici)

    private float scale;

    private float distanceDrawn = 0;
    private float[] minsqrdist;// Distances au carré minimales entre le dessin et le point de la forme
    private float avgsqrdist = float.MaxValue;// Moyenne de minsqrdist

    private LineRenderer baseLine;

    public Shape shape;

    public ShowValue agvsqdistText;
    

    // Start is called before the first frame update
    void Start()
    {
        // Initialisation des variables
        baseLine = GetComponent<LineRenderer>();

        //SetShape(shapeName);
    }

    public void SetShape(string shapeName)
    {
        // On récupère les données du fichier JSON correspondant au shapeName
        var textFile = Resources.Load<TextAsset>("Shapes/" + shapeName);
        if (textFile != null)
        {
            string jsonString = textFile.text;
            this.shape = JsonUtility.FromJson<Shape>(jsonString);
            DrawBaseShape(this.shape.points);
        }
        else
        {
            print("Erreur lors de la lecture du fichier : " + "Shapes/" + shapeName + ".json");
        }
        
    }

    public void EraseDrawnLines()
    {
        foreach (GameObject lineObject in GameObject.FindGameObjectsWithTag("DrawnLine")){
            Destroy(lineObject);
        }

        // Also reset variables
        distanceDrawn = 0;
        avgsqrdist = float.MaxValue;
        minsqrdist = new float[minsqrdist.Length];
        SetVisualsFromVariables();
    }

    public void DrawBaseShape(float[] shapePoints)
    {
        Rect rect = GetComponent<RectTransform>().rect; // En WorldGUI
        int nPoints = shapePoints.Length / 2;

        float xmin = rect.min.x;
        float ymin = rect.min.y;
        float xmax = rect.max.x;
        float ymax = rect.max.y;

        scale = Mathf.Min(xmax - xmin, ymax - ymin)/2;

        distanceDrawn = 0;
        points = new Vector3[nPoints];
        screenpoints = new Vector2[nPoints];
        minsqrdist = new float[nPoints];
        for (int i = 0; i < nPoints; i++)
        {
            points[i].x = scale*shapePoints[2*i]; // On passe de coord Shape en WorldGUI (relatif)
            points[i].y = scale*shapePoints[2*i+1];
            minsqrdist[i] = float.MaxValue;
            // On doit pourvoir trouver plus simple, mais au moins ça marche (on obtient les points en coordonnées de l'écran en passant par les coordonnées World)
            screenpoints[i] = Camera.main.WorldToScreenPoint(baseLine.transform.TransformPoint(points[i]));

        }
        print(scale);
        baseLine.positionCount = nPoints;
        baseLine.SetPositions(points);
        SetVisualsFromVariables();
    }

    public void EraseAll()
    {
        EraseDrawnLines();
        baseLine.positionCount = 0;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        FinishLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouse_over)
        {
            StartLine();
        }
        if (Input.GetMouseButtonUp(0))
        {
            FinishLine();
        }
    }

    void StartLine()
    {
        if (drawing != null) StopCoroutine(drawing);
        drawing = StartCoroutine(DrawLine());
    }

    void FinishLine()
    {
        StopCoroutine(drawing);
    }

    void SetVisualsFromVariables()
    {
        Color linecol = baseLine.material.color;
        linecol.a = Mathf.Lerp(1, 0, distanceDrawn / shape.length);
        baseLine.material.color = linecol;

        // Minsqdist
        if (agvsqdistText != null)
        {
            agvsqdistText.setValue(avgsqrdist.ToString());
        }
    }

    IEnumerator DrawLine()
    {
        LineRenderer newLine = Instantiate(Resources.Load("Line") as GameObject, new Vector3(0,0,0), Quaternion.identity).GetComponent<LineRenderer>();
        newLine.positionCount = 0;
        Vector2 mousePos;
        Vector2 lastPos = Vector2.zero;// Si non initialisée, engendre une erreur de compilation (mais cette valeur n'est jamais utilisée)

        while (true)
        {
            mousePos = Input.mousePosition; // En coordonnées de l'écran
            //mousePos.z = 0;
            Vector3 CameraMousePos = Camera.main.ScreenToWorldPoint(mousePos); // En coordonnées World (pour la ligne)
            CameraMousePos.z = 0;

            if (mouse_over){
                //print(line.positionCount);
                if (newLine.positionCount > 0)
                {
                    distanceDrawn += (mousePos - lastPos).magnitude / scale;// Distance à l'échelle de la forme
                    for (int i = 0; i < points.Length; i++)
                    {
                        float sqdist = ((mousePos - screenpoints[i])/scale).sqrMagnitude;
                        if (minsqrdist[i] > sqdist)
                        {
                            minsqrdist[i] = sqdist;
                        }
                    }
                    float sum = 0;
                    for (int i = 0; i < minsqrdist.Length; i++)
                    {
                        sum += minsqrdist[i];
                    }
                    avgsqrdist = sum / minsqrdist.Length;
                    print(avgsqrdist);
                    SetVisualsFromVariables();
                }
                newLine.positionCount++;
                newLine.SetPosition(newLine.positionCount - 1, CameraMousePos);
                lastPos = mousePos;
            }

            yield return null;
        }
    }
}
