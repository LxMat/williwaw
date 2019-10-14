﻿using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

[System.Obsolete]
public class Boat : NetworkBehaviour
{


    private Rigidbody boat;
    public GameObject follow;
    public GameObject micObject;
    public GameObject gyroObject;
    private float force;
    private float rotation;
    private Vector3 forceVector = Vector3.up;
    private Vector3 direction;
    private bool development = false;
    private WaterPlane waves;


    // Start is called before the first frame update
    private void Start()
    {
        boat = GetComponent<Rigidbody>();
        direction = new Vector3();
        development = false;
        transform.rotation = Quaternion.identity; //for some reason the boat spawns sideways...

    }

    public override void OnStartLocalPlayer()
    {
        //Camera.main.GetComponent<SmoothFollow>().target = follow.transform;
    }

    //Awake is called after all objects are initialized.
    void Awake()
    {
        waves = GameObject.Find("Waves").GetComponent<WaterPlane>();
        micObject = GameObject.Find("Microphone");
        gyroObject = GameObject.Find("Gyroscope");
    }

    Vector3 getNormal(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Vector3 u = p1 - p0;
        Vector3 v = p2 - p1;
        return Vector3.Cross(u, v).normalized;
    }

    private void setHeight(Transform gameObj)
    {

        Vector3 current = gameObj.position;
        gameObj.position = new Vector3(current.x, waves.getHeight(gameObj.position), current.z);
    }


    // Update is called once per frame
    private void Update()
    {
        if (isLocalPlayer)
        {

            Vector3 currentRot = transform.rotation.eulerAngles;
            Vector3 currentPos = transform.position;
            //Vector3 newPos = new Vector3(currentPos.x, waves.getHeight(currentPos), currentPos.z);
            //transform.position = newPos;
            //            transform.rotation = Quaternion.Euler(new Vector3(-90, currentRot.y, currentRot.z)); // stops boat from flipping around.


            force = micObject.GetComponent<MicrophoneInput>().force;
            rotation = -gyroObject.GetComponent<GyroscopeInput>().rotation;
            direction.z = -rotation;//Mathf.Sin(rotation);
                                    // direction.z = Mathf.Cos(rotation);

            forceVector = Vector3.up * force * 2.0f;

            if (boat.velocity.magnitude < 50)
            {
                boat.AddRelativeForce(forceVector);
            }


            //boat.AddTorque(direction);

            //transform.Rotate(direction, Space.Self);
            if (Input.GetKey(KeyCode.O))
            {
                development = !development;
            }
            if (true)
            {
                if (Input.GetKey(KeyCode.W))
                {
                        boat.AddRelativeForce(-Vector3.right);
                    
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Rotate(-Vector3.up);
                }
               if (Input.GetKey(KeyCode.D))
                {
                    transform.Rotate(Vector3.up);
                }
               if (Input.GetKey(KeyCode.S))
                {
                    boat.AddRelativeForce(Vector3.right);
                }
               
               
                    boat.velocity = boat.velocity * 0.995f;
               
            }
            //else
            //{
            //    if (rotation < -0.2f)
            //    {
            //        transform.Rotate(Vector3.forward);
            //    }
            //    if (rotation > 0.2f)
            //    {
            //        transform.Rotate(-Vector3.forward);
            //    }
            //}
        }
    }
}
