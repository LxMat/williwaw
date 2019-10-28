using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class ObjectiveManager : MonoBehaviour
{

    private int nObjectives = 5;
    private WaterPlane waves;
    public GameObject objectivePrefab;
    private int boundsX;
    private int boundsZ;
    // Start is called before the first frame update
    void Start()
    {

        
        
        for (int i = 0; i<nObjectives; i++)
        {
            SpawnObjective();
        }
    }

    void Awake()
    {
        waves = GameObject.Find("Waves").GetComponent<WaterPlane>();
        boundsX = waves.boundsX;
        boundsZ = waves.boundsZ;
    }

    void SpawnObjective()
    {
        GameObject objective;
        objective = Instantiate(objectivePrefab, new Vector3(Random.Range(50, 950), 200, Random.Range(50, 950)), Quaternion.identity);
        objective.transform.parent = transform;
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
