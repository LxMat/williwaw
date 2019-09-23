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
    private bool development;
    // Start is called before the first frame update
    void Start()
    {
        boat = GetComponent<Rigidbody>();
        direction = new Vector3();
        development = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        force = micObject.GetComponent<MicrophoneInput>().force;
        rotation = -gyroObject.GetComponent<GyroscopeInput>().rotation;
        direction.z = -rotation;//Mathf.Sin(rotation);
        // direction.z = Mathf.Cos(rotation);
        forceVector.y = force * 1.5f;

        if (boat.velocity.magnitude < 5)
        {
            boat.AddRelativeForce(forceVector);
        }
          

        //boat.AddTorque(direction);

        //transform.Rotate(direction, Space.Self);

        if (Input.GetKey(KeyCode.O))
        {
            development = !development;
        }
        if (development)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (boat.velocity.magnitude < 5)
                {
                    boat.AddRelativeForce(Vector3.up * 2);
                }
            }
            if (Input.GetKey(KeyCode.A) )
            {
                transform.Rotate(-Vector3.forward);
            }
            if (Input.GetKey(KeyCode.D) )
            {
                transform.Rotate(Vector3.forward);
            }
            if (Input.GetKey(KeyCode.S))
            {
                boat.AddRelativeForce(-Vector3.up);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                boat.AddRelativeForce(Vector3.forward);
            }
        }
        else
        {
            if (rotation < -0.2f )
            {
                transform.Rotate(-Vector3.forward);
            }
            if (rotation > 0.2f )
            {
                transform.Rotate(Vector3.forward);
            }
        }
       
        
    }
}
