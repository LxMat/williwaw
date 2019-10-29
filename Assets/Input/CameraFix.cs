using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFix : MonoBehaviour
{

    private WaterPlane waterPlane;
    // Start is called before the first frame update
    void Start()
    {
        waterPlane = GameObject.Find("Waves").GetComponent<WaterPlane>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tmp = transform.position;
        float h = waterPlane.getHeight(transform.position);
        if(tmp.y < h)
        {
            tmp.y = h + 1;
            transform.position = tmp;
        }
    }
}
