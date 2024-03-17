using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class positionscript : MonoBehaviour
{
    public float width = 2f;
    public float height = 2f;
    public GameObject bandesim;
    private float refpos;
    private Vector3 drawStartPosition;
    List<int> seqlength = new List<int>();
    public int niveau;


    // Start is called before the first frame update
    void Start()
    {
        /*for (int k = 0; k< spawnereaction.sequences.Count; k++){
            seqlength.Add(spawnereaction.sequences[k].Length);
            //Debug.Log("Add " + spawnereaction.sequences[k].Length +"for k = " + k);
        }
        niveau = spawnereaction.niveau;
        CalculateRefPosition();*/
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireCube(drawStartPosition, new Vector3(width, height, 0));
        //Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    void CalculateRefPosition()
    {
        refpos = bandesim.transform.position.x - (spawnereaction.sequences[spawnereaction.niveau].Length + 0.5f) * 1.5f - bandesim.transform.localScale.x - 2.0f;
        drawStartPosition = new Vector3(refpos, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (niveau < spawnereaction.niveau){
            niveau = spawnereaction.niveau;
            CalculateRefPosition();
        }*/
        
    }
}
