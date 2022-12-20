using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Newtonsoft.Json;// Pour le JSON "complexe"

public class Shape
{
    public string name;
    public float[][] points;
    public bool[] isLoop;
    public float length;
}

public class Draw : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Coroutine drawing;
    private bool mouse_over = false;

    private int totalBasePoints;
    private Vector3[] concat_points; // Points de la forme (coordonn�es WorldGUI relatives � l'�l�ment de GUI) : les diff�rents traits sont ici concat�n�s
    private Vector2[] concat_screenpoints; // Idem (coordonn�es absolues; Vector2 suffit ici)

    private float scale;

    private float distanceDrawn = 0;
    private int pointsDrawn = 0;

    private float[] rappr_minsqrdist; // Distances au carr� minimales entre le dessin et le point de la forme
    private float rappr_avgsqrdist = float.MaxValue; // Moyenne de minsqrdist

    private float ecart_sumsqrdist = 0;
    private float ecart_avgsqrdist = float.MaxValue;

    private GameObject[] clones;//Necessaires pour cr�er plusieurs lignes

    public Shape shape;

    public ShowValue rapprText;
    public ShowValue ecartText;
    public ShowValue scoreText;
    private GameObject hand;

    public Toggle transparencyMode;

    public Material baseLineMat;
    


    // Start is called before the first frame update
    void Start()
    {
        clones = new GameObject[0];
        hand = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    public void SetShape(string shapeName)
    {
        EraseAll();
        // On r�cup�re les donn�es du fichier JSON correspondant au shapeName
        var textFile = Resources.Load<TextAsset>("Shapes/" + shapeName);
        if (textFile != null)
        {
            string jsonString = textFile.text;
            this.shape = JsonConvert.DeserializeObject<Shape>(jsonString);

            Color color = baseLineMat.color;
            color.a = 1;
            baseLineMat.color = color;

            DrawBaseShape(this.shape.points,this.shape.isLoop);
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

    public void DrawBaseShape(float[][] shapePointsArrays, bool[] isLoop)
    {
        // On supprime les clones pr�c�dents
        foreach (GameObject clone in clones) {
            Object.Destroy(clone);
        }

        // Nouveaux clones
        clones = new GameObject[shapePointsArrays.Length];
        totalBasePoints = 0; // Nombre de points de la figure au total
        for (int i = 0; i < shapePointsArrays.Length; i++)
        {
            clones[i] = Instantiate(Resources.Load("baseLine") as GameObject, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            if (isLoop[i]) // Le path doit �tre dessin� comme une boucle
            {
                clones[i].GetComponent<LineRenderer>().loop = true;
            }
            float[] pathPoints = shapePointsArrays[i];
            totalBasePoints += pathPoints.Length / 2;// Attention � ne pas compter les deux coordonn�es (x et y), d'ou le /2
        }

        concat_points = new Vector3[totalBasePoints];
        concat_screenpoints = new Vector2[totalBasePoints];
        rappr_minsqrdist = new float[totalBasePoints];

        Rect rect = GetComponent<RectTransform>().rect; // En WorldGUI
        

        float xmin = rect.min.x;
        float ymin = rect.min.y;
        float xmax = rect.max.x;
        float ymax = rect.max.y;

        scale = Mathf.Min(xmax - xmin, ymax - ymin)/2;

        EraseDrawnLines();

        int nPointsSeen = 0;// Nombre de points d�j� ajout�s gr�ce aux paths pr�c�dents

        for (int ipath = 0; ipath < shapePointsArrays.Length; ipath++)
        {
            LineRenderer baseLine = clones[ipath].GetComponent<LineRenderer>();
            int nPoints = shapePointsArrays[ipath].Length / 2;

            Vector3[] points = new Vector3[nPoints];
            for (int i = 0; i < nPoints; i++)
            {
                points[i].x = scale * shapePointsArrays[ipath][2 * i]; // On passe de coord Shape en WorldGUI (relatif)
                points[i].y = scale * shapePointsArrays[ipath][2 * i + 1];
                rappr_minsqrdist[nPointsSeen + i] = float.MaxValue;

                concat_points[nPointsSeen + i] = points[i];
                // On doit pourvoir trouver plus simple, mais au moins �a marche (on obtient les points en coordonn�es de l'�cran en passant par les coordonn�es World)
                concat_screenpoints[nPointsSeen + i] = Camera.main.WorldToScreenPoint(baseLine.transform.TransformPoint(points[i]));

            }
            baseLine.positionCount = nPoints;
            baseLine.SetPositions(points);
            nPointsSeen += nPoints;
        }
        
        SetVisualsFromVariables();
    }

    public void EraseAll()
    {
        EraseDrawnLines();
        foreach (GameObject clone in clones)
        {
            Object.Destroy(clone);
        }
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
        if (concat_points != null)
        {
            drawing = StartCoroutine(DrawLine());
            hand.GetComponent<ParticleSystem>().Play();
        }
    }

    void FinishLine()
    {
        if (drawing != null) StopCoroutine(drawing);
        hand.GetComponent<ParticleSystem>().Stop();
    }

    void SetVisualsFromVariables()
    {

        // Calcul du score
        double sum = rappr_avgsqrdist + ecart_avgsqrdist;
        double exigence = 50;
        float alpha = 0.3F;
        double score = System.Math.Pow(1-System.Math.Tanh(exigence*sum),alpha);
        
        scoreText.setValue(score.ToString("P"));
        if (shape != null && transparencyMode.isOn)
        {
            Color linecol = baseLineMat.color;
            linecol.a = Mathf.Lerp(1, 0, distanceDrawn / (2 * shape.length));
            baseLineMat.color = linecol;
        }
        

        // Mise � jour des textes
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
        Vector2 lastPos = Vector2.zero;// Si non initialis�e, engendre une erreur de compilation (mais cette valeur n'est jamais utilis�e)

        while (true)
        {
            if (mouse_over){
                mousePos = Input.mousePosition; // En coordonn�es de l'�cran
                Vector3 CameraMousePos = Camera.main.ScreenToWorldPoint(mousePos); // En coordonn�es World (pour la ligne)
                CameraMousePos.z = 0;

                // Ajout d'un nouveau point dessin�
                pointsDrawn++;
                newLine.positionCount++;
                newLine.SetPosition(newLine.positionCount - 1, CameraMousePos);

                hand.transform.position = CameraMousePos;
                
                
                // Au moins le deuxi�me point de la ligne dessin�e
                if (newLine.positionCount > 1)
                {
                    float distToPreviousPoint = (mousePos - lastPos).magnitude / scale;
                    distanceDrawn += distToPreviousPoint;// Distance � l'�chelle de la forme

                    // Mise � jour du calcul du rapprochement et d'�cartement (on a une pond�ration par la distance entre deux points, dont on ne peut pas inclure le premier point)
                    float ecart_minsqrdist = float.MaxValue;
                    for (int i = 0; i < totalBasePoints; i++)
                    {
                        float sqrdist = ((mousePos - concat_screenpoints[i]) / scale).sqrMagnitude;
                        if (rappr_minsqrdist[i] > sqrdist) rappr_minsqrdist[i] = sqrdist;
                        if (sqrdist < ecart_minsqrdist) ecart_minsqrdist = sqrdist;
                    }
                    ecart_sumsqrdist += ecart_minsqrdist*distToPreviousPoint;
                    ecart_avgsqrdist = ecart_sumsqrdist / distanceDrawn;

                    float sum = 0;
                    for (int i = 0; i < totalBasePoints; i++)
                    {
                        sum += rappr_minsqrdist[i];
                    }
                    rappr_avgsqrdist = sum / totalBasePoints;
                    SetVisualsFromVariables();
                }
                
                lastPos = mousePos;
            }

            yield return null;
        }
    }
}
