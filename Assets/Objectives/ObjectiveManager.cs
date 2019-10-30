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

        for (int i = 0; i < nObjectives; i++)
        {
            if (isServer)
            {
                SpawnObjective(new Vector3(Random.Range(50, 950), 0, Random.Range(50, 950)));
            }

        }


    }

    private void Update()
    {


        if (transform.childCount < 5)
        {
            if (isServer)
            {
                SpawnObjective(new Vector3(Random.Range(50, 950), 0, Random.Range(50, 950)));
            }


        }

    }

    void SpawnObjective(Vector3 position)
    {
        GameObject objective = Instantiate(objectivePrefab, position, Quaternion.identity);
        NetworkServer.Spawn(objective);
        objective.transform.parent = transform;
    }
}
