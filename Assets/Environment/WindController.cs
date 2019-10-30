using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class WindController : NetworkBehaviour
{
    // Start is called before the first frame update
    [SyncVar]
    public float power = 0.3f;

    [SyncVar]
    public Vector3 direction = new Vector3(0.0f, 0.0f, 1.0f);

    private WindZone windZone;

    void Start()
    {
        windZone = transform.GetComponent<WindZone>();
        transform.rotation = Quaternion.LookRotation(direction);
    }

    [Command]
    public void CmdSetWind(Vector3 newDirection, float newPower)
    {
        direction = newDirection;
        power = newPower;
    }

    // Update is called once per frame
    void Update()
    {
        windZone.windMain = power * 10;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
