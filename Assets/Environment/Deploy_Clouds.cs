using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class Deploy_Clouds : NetworkBehaviour
{
    public GameObject CloudPrefab;
    private readonly int cloudHeight = 100;

    public int nClouds = 10;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < nClouds; i++)
        {
            spawnCloud(new Vector3(Random.Range(50, 950), cloudHeight, Random.Range(50, 950)));
        }
    }
    public void spawnCloud(Vector3 pos)
    {
        if (isServer)
        {
            Debug.Log("Cloud");
            GameObject cloud = Instantiate(CloudPrefab, new Vector3(pos.x, cloudHeight, pos.z), Quaternion.identity);
            NetworkServer.Spawn(cloud);
            cloud.transform.parent = transform;
        }
    }
}
