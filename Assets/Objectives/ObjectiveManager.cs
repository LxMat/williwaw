using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class ObjectiveManager : NetworkBehaviour
{

    private int nObjectives = 5;
    public int score;
    private WaterPlane waves;
    public GameObject objectivePrefab;
    private Boat player;
    // Start is called before the first frame update
    void Start()
    {
        score = -nObjectives;
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
        score += 1;
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

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(500, 100, 300, 40), "Score: " + score);
        GUI.skin.label.fontSize = 16;
    }

}
