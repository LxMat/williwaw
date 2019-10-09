using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boattilt : MonoBehaviour
{
    private WaterPlane waves;
    private Transform boat;
    private Transform s1; //sphere 1, used for aligning boat to the waves
    private Transform s2;
    private Transform s3;
    private Transform s4;
    

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount < 5)
        {
            Debug.LogWarning(this.name + ": Ship assets is not properly set");
        }


        waves = GameObject.Find("Waves").GetComponent<WaterPlane>();


        boat = transform.GetChild(0);
        s1 = transform.GetChild(1);
        s2 = transform.GetChild(2);
        s3 = transform.GetChild(3);
        s4 = transform.GetChild(4);

        //we don't want the spheres to render
        Renderer[] spheres = transform.GetComponentsInChildren<Renderer>();
        for (int i = 1; i <= 4; i++)
        {
            spheres[i].enabled = false;
        }
    }
    void Awake()
    {

    }
    private void setHeight(Transform gameObj)
    {

        Vector3 current = gameObj.position;
        gameObj.position = new Vector3(current.x, waves.getHeight(gameObj.position), current.z);
    }
    Vector3 getNormal(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Vector3 u = p1 - p0;
        Vector3 v = p2 - p1;
        return Vector3.Cross(u,v).normalized;
    }

    void Update()
    {

        setHeight(boat);
        setHeight(s1);
        setHeight(s2);
        setHeight(s3);
        setHeight(s4);
        

        Vector3 normal = getNormal(s1.position, s3.position, s2.position);
        Vector3 normal2 = getNormal(s4.position, s3.position, s2.position);
        boat.rotation = Quaternion.FromToRotation(Vector3.up, ((normal + normal2) / 2)) * transform.rotation ;
    }
}
