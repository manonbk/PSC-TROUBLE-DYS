using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
//[RequireComponent(typeof(MeshRenderer))]

public class DrawRefBande : MonoBehaviour
{
    List<int> seqlength = new List<int>();
    //public float width = 2f;
    public float length = 10f; 

    public GameObject bandesim;
    public static float banderef;

    public static float dbesoin = 0;

    static bool hasdrew = false;
    public static float simpos;
    public static float simscale;

    public static Mesh squareMesh;


    void Start()
    {
        //Debug.Log("seq Count" + spawnereaction.sequences.Count);

        for (int k = 0; k< spawnereaction.sequences.Count; k++){
            seqlength.Add(spawnereaction.sequences[k].Length);
            //Debug.Log("Add " + spawnereaction.sequences[k].Length +"for k = " + k);
        }

        simpos = bandesim.transform.position.x;
        simscale = bandesim.transform.localScale.x;
        //Debug.Log("simpos drawbande:" + simpos);


        GenerateSquare();
    }

    void Update(){
        if(!hasdrew){
            GenerateSquare();
        }
    }


    void GenerateSquare()
    {
        //Debug.Log("Seqlength count:" + seqlength.Count);
        //Debug.Log("niveau:" + spawnereaction.niveau);
        //Debug.Log("GenerateSquare:" + seqlength[spawnereaction.niveau]);

        dbesoin = (seqlength[spawnereaction.niveau] + 0.5f) * 1.5f; //donner 1/2 fois cercle de marge
        banderef = simpos - simscale - dbesoin;


        MeshFilter meshFilter = GetComponent<MeshFilter>();
        //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        squareMesh = new Mesh();

        // Vertices of the square
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(banderef - simscale / 2, -length / 2, 0);
        vertices[1] = new Vector3(banderef - simscale / 2, length / 2, 0);
        vertices[2] = new Vector3(banderef + simscale / 2, length / 2, 0);
        vertices[3] = new Vector3(banderef + simscale / 2, -length / 2, 0);

        // Triangles
        int[] triangles = new int[6] { 0, 1, 2, 0, 2, 3 };

        // Normals
        Vector3[] normals = new Vector3[4];
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = Vector3.forward;
        }

        // UVs
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        squareMesh.vertices = vertices;
        squareMesh.triangles = triangles;
        squareMesh.normals = normals;
        squareMesh.uv = uv;

        meshFilter.mesh = squareMesh;

        hasdrew = true;

    }

    public static void Reset(){
        banderef = 0;
        dbesoin = 0;
        hasdrew = false;
        if (squareMesh != null)
        {
            Destroy(squareMesh);
            squareMesh = null;
        }
        else{
            Debug.Log("No square to destroy");
        }
    }
}
