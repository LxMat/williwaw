using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    // Start is called before the first frame update

    public float power = 0.3f;
    public Vector3 direction = new Vector3(0.0f, 0.0f, 1.0f);

    private WindZone windZone;

    void Start()
    {
        windZone = transform.GetComponent<WindZone>();
        transform.rotation = Quaternion.LookRotation(direction);
    }

    // Update is called once per frame
    void Update()
    {
        windZone.windMain = power * 20;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
