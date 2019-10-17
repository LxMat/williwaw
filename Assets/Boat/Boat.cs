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
    public GameObject CannonBallPrefab;
    public float CannonBallSpeed;
    private float force;
    private float rotation;
    private Vector3 forceVector = Vector3.up;
    private Vector3 direction;
    private bool development = false;
    private WaterPlane waves;

    //variables used for the camera
    private float cameraDistanceInit,cameraHeightInit;
    private float cameraDistanceSpeedUp = 1000f;
    private SmoothFollow boatCamera;
    private float currentCameraDistance;
    private int health = 1;
    private float cooldown = 2f;
    private float nextAttack;
    private float cameraSpeedThreshold = 20f;
    // Start is called before the first frame update
    private Transform cylinder;
    private void Start()
    {
        boat = GetComponent<Rigidbody>();
        direction = new Vector3();
        development = false;
        transform.rotation = Quaternion.identity; //for some reason the boat spawns sideways...
        boatCamera = Camera.main.GetComponent<SmoothFollow>();
        cameraDistanceInit = boatCamera.distance;


        cylinder = transform.GetChild(7);
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<SmoothFollow>().target = follow.transform;
        cameraDistanceInit = currentCameraDistance = Camera.main.GetComponent<SmoothFollow>().distance;
        cameraHeightInit = Camera.main.GetComponent<SmoothFollow>().height;
        Debug.Log("height: "+ cameraHeightInit);

    }

    private void KillPlayer()
    {
        Destroy(gameObject);
        Debug.Log("You are dead");
    }

    private void ShootLaser()
    {
        nextAttack = Time.time + cooldown;
        CmdShoot();
        Debug.Log("BOOM");
    }

    [Command] void CmdShoot()
    {
        GameObject cannonBall = Instantiate(CannonBallPrefab, transform.position + transform.right * -10, transform.rotation);
        cannonBall.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-CannonBallSpeed, 75, 0));
        NetworkServer.Spawn(cannonBall);
        Debug.Log("BOOM");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            health--;
            Debug.Log("Health left: " + health);
            if (health == 0)
            {
                KillPlayer();
            }
        }
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
            setHeight(transform);
            force = micObject.GetComponent<MicrophoneInput>().force;
            rotation = gyroObject.GetComponent<GyroscopeInput>().rotation;
            direction.z = -rotation;//Mathf.Sin(rotation);
                                    // direction.z = Mathf.Cos(rotation);


            forceVector = -Vector3.right * force * 2.0f;

            if (boat.velocity.magnitude < 50)
            {
                boat.AddRelativeForce(forceVector);
                
            }


            //if the boat speeds up the camera moves further away and as is slows down the camera gets closer.
            if (boat.velocity.magnitude > cameraSpeedThreshold && boatCamera.distance < (cameraDistanceSpeedUp)) 
            {
                boatCamera.distance = Mathf.Lerp(boatCamera.distance,200f,Time.deltaTime/10);
                boatCamera.height = Mathf.Lerp(boatCamera.height, cameraHeightInit + 5, Time.deltaTime);
            }
            else if (boat.velocity.magnitude <= cameraSpeedThreshold && boatCamera.distance != cameraDistanceInit)
            {
                boatCamera.distance = Mathf.Lerp(boatCamera.distance, cameraDistanceInit, Time.deltaTime);
                boatCamera.height = Mathf.Lerp(boatCamera.height, cameraHeightInit, Time.deltaTime);
            }

            //boat.AddTorque(direction);

            //transform.Rotate(direction, Space.Self);

            if (Input.GetKey(KeyCode.Space) && nextAttack < Time.time)
            {
                ShootLaser();
            }


            if (Input.GetKey(KeyCode.O))
            {
                development = !development;
            }
            if (development) 
            {
                if (Input.GetKey(KeyCode.W))
                {
                    if (boat.velocity.magnitude < 50)
                    {
                        boat.AddRelativeForce(-Vector3.right * 2);
                    }
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
            else
            {
                gyroTest(); 
                
                if (Mathf.Abs(rotation) >0.5f)
                {
                    transform.Rotate(0, rotation, 0);
                }
                //if (rotation > 0.2f)
                //{
                //    transform.Rotate(-Vector3.forward);
                //}
            }
        }
    }



    void gyroTest()
    {
        Gyroscope gyro = Input.gyro;
        if (gyro == null) { return; }
        cylinder.rotation = gyro.attitude;
    }

}
