using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{

    Rigidbody boat;
    public GameObject micObject;
    public GameObject gyroObject;
    private float force;
    private float rotation;
    private Vector3 forceVector = Vector3.up;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        boat = GetComponent<Rigidbody>();
        direction = new Vector3();
        
    }

    // Update is called once per frame
    void Update()
    {
        force = micObject.GetComponent<MicrophoneInput>().force;
        rotation = gyroObject.GetComponent<GyroscopeInput>().rotation;
        direction.z = -rotation;//Mathf.Sin(rotation);
        // direction.z = Mathf.Cos(rotation);
        forceVector.y = force * 1f;


        boat.AddRelativeForce(forceVector);

        
       
        transform.Rotate(direction, Space.Self);

        if (Input.GetKey(KeyCode.W))
        {
            boat.AddRelativeForce(Vector3.up);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            boat.AddRelativeForce(Vector3.forward);
        }
    }
}
