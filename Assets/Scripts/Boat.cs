using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{

    Rigidbody boat;
    // Start is called before the first frame update
    void Start()
    {
        boat = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            boat.AddRelativeForce(Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            boat.AddRelativeForce(-Vector3.up);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            boat.AddRelativeForce(Vector3.forward);
        }
    }
}
