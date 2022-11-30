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
    private int pointsDrawn = 0;

    private float[] rappr_minsqrdist; // Distances au carré minimales entre le dessin et le point de la forme
    private float rappr_avgsqrdist = float.MaxValue; // Moyenne de minsqrdist

    private float ecart_sumsqrdist = 0;
    private float ecart_avgsqrdist = float.MaxValue;


    private LineRenderer baseLine;

    public Shape shape;

    public ShowValue rapprText;
    public ShowValue ecartText;
    

    // Start is called before the first frame update
    void Start()
    {
        // Initialisation des variables
        baseLine = GetComponent<LineRenderer>();

        //SetShape(shapeName);
    }

    public void SetShape(string shapeName)
    {
        EraseAll();
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
        pointsDrawn = 0;
        rappr_avgsqrdist = float.MaxValue;
        if (rappr_minsqrdist != null)
        {
            rappr_minsqrdist = new float[rappr_minsqrdist.Length];
            for (int i = 0; i < rappr_minsqrdist.Length; i++)
            {
                rappr_minsqrdist[i] = float.MaxValue;
            }
        }
        ecart_sumsqrdist = 0;
        ecart_avgsqrdist = float.MaxValue;
        
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
        pointsDrawn = 0;
        points = new Vector3[nPoints];
        screenpoints = new Vector2[nPoints];
        rappr_minsqrdist = new float[nPoints];
        for (int i = 0; i < nPoints; i++)
        {
            points[i].x = scale*shapePoints[2*i]; // On passe de coord Shape en WorldGUI (relatif)
            points[i].y = scale*shapePoints[2*i+1];
            rappr_minsqrdist[i] = float.MaxValue;
            // On doit pourvoir trouver plus simple, mais au moins ça marche (on obtient les points en coordonnées de l'écran en passant par les coordonnées World)
            screenpoints[i] = Camera.main.WorldToScreenPoint(baseLine.transform.TransformPoint(points[i]));

        }
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
        if (points != null) drawing = StartCoroutine(DrawLine());
    }

    void FinishLine()
    {
        if (drawing != null) StopCoroutine(drawing);
    }

    void SetVisualsFromVariables()
    {
        if (shape != null)
        {
            Color linecol = baseLine.material.color;
            linecol.a = Mathf.Lerp(1, 0, distanceDrawn / (2 * shape.length));
            baseLine.material.color = linecol;
        }
        

        // Mise à jour des textes
        if (rapprText != null)
        {
            rapprText.setValue(rappr_avgsqrdist.ToString());
        }
        if (ecartText != null)
        {
            ecartText.setValue(ecart_avgsqrdist.ToString());
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
            if (mouse_over){
                mousePos = Input.mousePosition; // En coordonnées de l'écran
                Vector3 CameraMousePos = Camera.main.ScreenToWorldPoint(mousePos); // En coordonnées World (pour la ligne)
                CameraMousePos.z = 0;

                // Ajout d'un nouveau point dessiné
                pointsDrawn++;
                newLine.positionCount++;
                newLine.SetPosition(newLine.positionCount - 1, CameraMousePos);
                
                
                // Au moins le deuxième point de la ligne dessinée
                if (newLine.positionCount > 1)
                {
                    float distToPreviousPoint = (mousePos - lastPos).magnitude / scale;
                    distanceDrawn += distToPreviousPoint;// Distance à l'échelle de la forme

                    // Mise à jour du calcul du rapprochement et d'écartement (on a une pondération par la distance entre deux points, dont on ne peut pas inclure le premier point)
                    float ecart_minsqrdist = float.MaxValue;
                    for (int i = 0; i < points.Length; i++)
                    {
                        float sqrdist = ((mousePos - screenpoints[i]) / scale).sqrMagnitude;
                        if (rappr_minsqrdist[i] > sqrdist) rappr_minsqrdist[i] = sqrdist;
                        if (sqrdist < ecart_minsqrdist) ecart_minsqrdist = sqrdist;
                    }
                    ecart_sumsqrdist += ecart_minsqrdist*distToPreviousPoint;
                    ecart_avgsqrdist = ecart_sumsqrdist / distanceDrawn;

                    float sum = 0;
                    for (int i = 0; i < rappr_minsqrdist.Length; i++)
                    {
                        sum += rappr_minsqrdist[i];
                    }
                    rappr_avgsqrdist = sum / rappr_minsqrdist.Length;
                    print(distanceDrawn);
                    SetVisualsFromVariables();
                }
                
                lastPos = mousePos;
            }

            yield return null;
        }
    }
}
