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
            spawnCloud();
        }
    }
    void spawnCloud()
    {
        if (isServer)
        {
            GameObject cloud = Instantiate(CloudPrefab, new Vector3(Random.Range(50, 950), cloudHeight, Random.Range(50, 950)), Quaternion.identity);
            NetworkServer.Spawn(cloud);
            cloud.transform.parent = transform;
        }
    }

    public void SpawnCloudsOnPlayer(Vector3 pos)
    {
        nClouds += 1;


        CmdCloud(new Vector3(pos.x, cloudHeight, pos.z));

    }

    [Command]
    void CmdCloud(Vector3 position)
    {
        GameObject cloud = Instantiate(CloudPrefab, position, Quaternion.identity);
        cloud.transform.parent = transform;
        NetworkServer.Spawn(cloud);
    }

}
