using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGates : MonoBehaviour
{
    List<int> seqlength = new List<int>();
    public static float startteleport;
    public static float endteleport;

    public GameObject banderef;
    public GameObject bandesim;
    public static float dbesoin = 0;

    private static LineRenderer linestart;
    private static LineRenderer lineend;

    public Material material;

    static bool hasdrew = false;

    public static float refpos;
    public static float refscale;
    public static float simpos;
    public static float simscale;

    // Start is called before the first frame update
    void Start()
    {
        for (int k = 0; k< SpawnerAction.sequences.Count; k++){
            seqlength.Add(SpawnerAction.sequences[k].Length);
        }
        /*seqlength.Add(2);
        seqlength.Add(3);
        seqlength.Add(3);
        seqlength.Add(5);
        seqlength.Add(4);
        seqlength.Add(5);
        seqlength.Add(4);
        seqlength.Add(5);
        seqlength.Add(6);
        seqlength.Add(8);
        seqlength.Add(6);
        seqlength.Add(7);
        seqlength.Add(6);
        seqlength.Add(5);
        seqlength.Add(7);
        seqlength.Add(7);
        seqlength.Add(8);
        seqlength.Add(8);
        seqlength.Add(9);
        seqlength.Add(9);
        seqlength.Add(10);
        seqlength.Add(11);
        seqlength.Add(12);*/

        //Debug.Log(seqlength);

        refpos = banderef.transform.position.x;
        simpos = bandesim.transform.position.x;
        refscale = banderef.transform.localScale.x;
        simscale = bandesim.transform.localScale.x;

        drawing();

   }

    // Update is called once per frame
    void Update()
    {
        if(!hasdrew){
            drawing();
        }
    }

    void drawing(){
        //Debug.Log("Start Drawing Gates");

        dbesoin = (seqlength[SpawnerAction.niveau] + 0.5f) * 1.5f; //donner 1/2 fois cercle de marge
        startteleport = refpos + refscale/2 + dbesoin/ 2;
        endteleport = simpos - simscale/2 - dbesoin/ 2;

        //Debug.Log("refpos: " + simpos);
        //Debug.Log("simscale: " + simscale);
        //Debug.Log("dbesoin: " + dbesoin);
        //Debug.Log("startteleport: " + startteleport);
        //Debug.Log("endteleport: " + endteleport);

        createLine(ref linestart, "StartGate");
        createLine(ref lineend, "EndGate");

        linestart.SetPosition(0, new Vector3(startteleport, 0f, 0));
        linestart.SetPosition(1, new Vector3(startteleport, 5f, 0));
        lineend.SetPosition(0, new Vector3(endteleport, 0f, 0));
        lineend.SetPosition(1, new Vector3(endteleport, -5f, 0));

        hasdrew = true;
    }

    public static void Reset(){
        startteleport = 0;
        endteleport = 0;
        dbesoin = 0;
        hasdrew = false;
        if (linestart != null && lineend != null){
            Destroy(linestart);
            Destroy(lineend);
        }
        else{
            Debug.Log("No line to destroy");
        }
    }

    void createLine(ref LineRenderer line, string str){
        line = new GameObject(str).AddComponent<LineRenderer>();
        line.material = material;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
    }
}