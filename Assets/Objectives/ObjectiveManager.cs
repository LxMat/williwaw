using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class ObjectiveManager : NetworkBehaviour
{

    private int nObjectives = 5;
    private WaterPlane waves;
    public GameObject objectivePrefab;
    // Start is called before the first frame update
    void Start()
    {

        waves = GameObject.Find("Waves").GetComponent<WaterPlane>();
        boundsX = waves.boundsX;
        boundsZ = waves.boundsZ;

        for (int i = 0; i<nObjectives; i++)
        {
            if (isServer)
            {
                SpawnObjective(new Vector3(Random.Range(50, 950), 200, Random.Range(50, 950)));
            }
            else
            {
                CmdObjective(new Vector3(Random.Range(50, 950), 200, Random.Range(50, 950)));
            }
        }


    }

    private void Update()
    {


        if (transform.childCount < 5)
        {
            if (isServer)
            {
                SpawnObjective(new Vector3(Random.Range(50, 950), 200, Random.Range(50, 950)));
            }
            else
            {
                CmdObjective(new Vector3(Random.Range(50, 950), 200, Random.Range(50, 950)));
            }

        }

    }

    void SpawnObjective(Vector3 position)
    {
        GameObject objective = Instantiate(objectivePrefab, position, Quaternion.identity);
        NetworkServer.Spawn(objective);
        objective.transform.parent = transform;
    }

    [Command]
    void CmdObjective(Vector3 position)
    {
        GameObject objective = Instantiate(objectivePrefab, position, Quaternion.identity);
        objective.transform.parent = transform;
        NetworkServer.Spawn(objective);
    }
}
