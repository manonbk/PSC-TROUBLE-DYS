using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShapesManager : MonoBehaviour
{

    // Script qui gère les images : fonctions pour dessiner, 

    public string[] shapeNames;
    public int[] shapeLevel;
    public bool [] shapeAchieved;
    public float[] shapeSizings;
    int currentShapeIndex = 0;
    public DrawTemplate drawTemplate; // C'est un script
    public Draw draw; // script aussi. Différence avec draw?
    public GameObject startButton;
    public GameObject endPanel;

    public double seuilMontant;
    public double seuilDescendant;
    private int niveau = 1;


    // Start is called before the first frame update

    private void Start()
    {
        Invoke("DelayedStart", .5f); // Executes the specified delegate on the thread that owns the control's underlying window handle. ?? je sais pas trop
    }

    void DelayedStart()
    {
        string shapeName = shapeNames[0];
        float shapeSizing = shapeSizings[0];
        drawTemplate.SetShape(shapeName); // appelle la fonction qui dessine la forme demandée (va chercher dans le script drawTemplate la fonction SetShape)
        draw.SetShape(shapeName, shapeSizing);
    }
 

    public void StartGame() // Quand estce qu'elle s'execute ?
    {
        draw.hasStarted = true;
        startButton.SetActive(false);
        if (currentShapeIndex == -1)
        {
            NextShape();
        }
    }

    void SetShapeIndex(int i) // permet d'aller une forme à lautre
    {
        currentShapeIndex = i;
        string shapeName = shapeNames[currentShapeIndex];
        float shapeSizing = shapeSizings[currentShapeIndex];
        drawTemplate.SetShape(shapeName);
        draw.SetShape(shapeName, shapeSizing);
    }

    public void NextShape() // on cherche la prochaine forme du niveau requis non déja faite
    {
        int potentialIndex = currentShapeIndex;
        bool found = false ; // si on a trouvé un bon indice
        bool fini = false; //si on a tout testé on retourne au menu
        while (!found && !fini)
        {
            potentialIndex= ((potentialIndex+1) % shapeNames.Length);
            if (potentialIndex==currentShapeIndex) {fini=true;} // on a fait le tour
            if (!shapeAchieved[potentialIndex] && shapeLevel[potentialIndex]==niveau) {found=true;}

        }
        if (fini) {SceneManager.LoadScene(0);} else {SetShapeIndex(potentialIndex);}
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
            shapeAchieved[currentShapeIndex]=true;
            NextShape();
        }
        else
        {
            double score = draw.CalculeScore();
            Debug.Log(score.ToString());
            if (score > seuilMontant && niveau!=3) {niveau++;}
            if (score < seuilDescendant && niveau!=1) {niveau--;}
            Debug.Log(niveau.ToString());
            draw.ConvertToImage();
        }
    }
}
