using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class WindManager : NetworkBehaviour
{

    public GameObject windPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject wind = Instantiate(windPrefab, new Vector3(500, 0, 500), Quaternion.identity);
        NetworkServer.Spawn(wind);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
