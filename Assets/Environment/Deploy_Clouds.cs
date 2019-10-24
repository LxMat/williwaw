using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deploy_Clouds : MonoBehaviour
{
    public GameObject CloudPrefab;

    public int nClouds = 10;
    private List<GameObject> cloudList;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < nClouds; i++)
        {
            spawnCloud();       
        }
    }
    void spawnCloud() {
        GameObject cloud;
        cloud = Instantiate(CloudPrefab, new Vector3(Random.Range(50, 950), 200, Random.Range(50,950)), Quaternion.identity);
        cloud.transform.parent = transform;
    }

    public void SpawnCloudsOnPlayer(Vector3 pos)
    {
        nClouds += 1;
        Debug.Log("Spawnedcloud");
        GameObject cloud;
        cloud = Instantiate(CloudPrefab, new Vector3(pos.x, 200, pos.z), Quaternion.identity);
        cloud.transform.parent = transform;
    }
  
}
