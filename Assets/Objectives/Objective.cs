using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class Objective : NetworkBehaviour
{

    private Material material;

    private WaterPlane waves;
    private Vector3 temp;

    [SyncVar]
    public string objectiveType;
    private List<string> list;



    //Materials:
    public Material cloudMaterial,
                    windMaterial,
                    waveMaterial;
    // Start is called before the first frame update
    void Start()
    {
        list = new List<string> { "Wind", "Cloud", "Wave" };


        //Assign a string as the type
        objectiveType = list[Random.Range(0, list.Count)];

        switch (objectiveType)
        {
            case "Wind":
                {
                    GetComponent<Renderer>().material = windMaterial;
                    break;
                }
            case "Cloud":
                {
                    GetComponent<Renderer>().material = cloudMaterial;
                    break;
                }
            case "Wave":
                {
                    GetComponent<Renderer>().material = waveMaterial;
                    break;
                }
        }
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        temp = transform.position;
        // temp.y = waves.getHeight(temp) + 5.0f;
        transform.position = temp;
    }
}
