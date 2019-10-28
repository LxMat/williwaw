using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{

    private WaterPlane waves;
    private Vector3 temp;
    public string objectiveType;
    private List<string> list;
    // Start is called before the first frame update
    void Start()
    {
        list = new List<string> { "Wind", "Cloud", "Wave" };
        //Assign a string as the type
        objectiveType = list[Random.Range(0, list.Count)];
    }
    private void Awake()
    {
        waves = GameObject.Find("Waves").GetComponent<WaterPlane>();
    }


    // Update is called once per frame
    void Update()
    {
        temp = transform.position;
        temp.y = waves.getHeight(temp);
        transform.position = temp;
    }
}
