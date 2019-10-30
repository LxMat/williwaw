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
    public LightSensorInput lightSensor;
    private Deploy_Clouds deployClouds;
    private WindController windController;
    private float force;
    private float rotation;
    private float pitch;
    private bool cloudSpawn;


    private float accu;
    private int n;


    private string powerType;

    private Vector3 forceVector;
    private Vector3 windVector;
    private Vector3 direction;
    private bool development;
    private WaterPlane waves;
    private Material waterShader;
    private Vector4 waveVector = new Vector4();

    private float windAngle;

    //variables used for the camera
    private float cameraDistanceInit, cameraHeightInit;
    private readonly float cameraDistanceSpeedUp = 1000f;
    private SmoothFollow boatCamera;
    private float currentCameraDistance;
    private readonly float cloudCooldown = 0.5f;
    private float nextCloud;
    private readonly float cameraSpeedThreshold = 20f;
    // Start is called before the first frame update
    private Transform cylinder;


    private Transform cloudTransform;
    private ParticleSystem cloudSystem;



    private WindZone windZone;


    private Transform perl;

    private void Start()
    {

        waves = GameObject.Find("Waves").GetComponent<WaterPlane>();
        waterShader = waves.material;

        
        micObject = GameObject.Find("Microphone");
        gyroObject = GameObject.Find("Gyroscope");
        windController = GameObject.Find("Wind").GetComponent<WindController>();
        
        lightSensor = GameObject.Find("LightSensor").GetComponent<LightSensorInput>();
      
        deployClouds = GameObject.Find("CloudManager").GetComponent<Deploy_Clouds>();


        perl = transform.FindChild("black_perl");

        boat = GetComponent<Rigidbody>();
        direction = new Vector3();
        development = false;
        transform.rotation = Quaternion.identity; //for some reason the boat spawns sideways...
        boatCamera = Camera.main.GetComponent<SmoothFollow>();
        cameraDistanceInit = boatCamera.distance;

        cloudTransform = transform.FindChild("Cloud");
        cloudSystem = cloudTransform.GetComponent<ParticleSystem>();
        cloudSystem.enableEmission = false;


        //windZone = transform.FindChild("WindZone").GetComponent<WindZone>();

        cylinder = transform.GetChild(7);
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<SmoothFollow>().target = follow.transform;
        cameraDistanceInit = currentCameraDistance = Camera.main.GetComponent<SmoothFollow>().distance;
        cameraHeightInit = Camera.main.GetComponent<SmoothFollow>().height;
        Debug.Log("height: " + cameraHeightInit);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {

            //if we have a power when we pick up a new one, just switch. 
            if (powerType != null)
            {
                ResetPower();
            }

            powerType = collision.gameObject.GetComponent<Objective>().objectiveType;
            Debug.Log(powerType);
            if (powerType == "Wave")
            {
                accu = 0.0f;
                n = 0;
                Invoke("SetWaves", 5.0f);
            }
            Invoke("ResetPower", 5.0f); //Alternativeley get differrent lengths from different powers. 
            Destroy(collision.gameObject);
        }
    }



    //Awake is called after all objects are initialized.
    void Awake()
    {

    }

    Vector3 GetNormal(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Vector3 u = p1 - p0;
        Vector3 v = p2 - p1;
        return Vector3.Cross(u, v).normalized;
    }

    private void SetHeight(Transform gameObj)
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
            SetHeight(transform);
           
            
            force = micObject.GetComponent<MicrophoneInput>().force;
            pitch = micObject.GetComponent<MicrophoneInput>().PitchValue;
            rotation = gyroObject.GetComponent<GyroscopeInput>().rotation;

            //windZone.windMain = force * 10; //SEts the force of the windzone on the boat. 

            //Calculate wind angle and resulting velocity. Maybe have a minimum velocity, and not just 0. 
            windAngle = (1- (Vector3.Angle(-transform.right, windController.direction) / 150.0f)) ; //0 degrees is -1, 30 degrees is 0 force, 180 degrees is 5. Normalised
            windVector = -perl.right * windAngle; //Force in the forward direction of the ship depending on windangle. Important! Can be negative
            windVector = windVector * windController.power; //power is between 0 and 1;
            forceVector = -perl.right * force; //The force in the forward direction as measured by the users blowing on the microphone. 

       

            //if (Mathf.Abs(rotation) > 0.2f)
            //{
                
            //    direction.z = -rotation;//Mathf.Sin(rotation);
            //                            // direction.z = Mathf.Cos(rotation);
            //}



          
           boat.velocity = Vector3.ClampMagnitude((forceVector + windVector )* 30, 30);
                
                //boat.AddRelativeForce(-Vector3.right * windAngle);

                //boat.AddRelativeForce(-Vector3.right * force);

        


            if (powerType != null)
            {
                if (powerType == "Cloud")
                {
                   
                    cloudSpawn = lightSensor.spawnCloud;
                    if (cloudSpawn && nextCloud < Time.time)
                    {
                        cloudSystem.enableEmission = true;
                        nextCloud = Time.time + cloudCooldown;
                        deployClouds.SpawnCloudsOnPlayer(transform.position);
                    }
                    else if (nextCloud + 0.2f < Time.time)
                    {
                        cloudSystem.enableEmission = false;
                    }
                }
              
                
                if (powerType == "Wind")
                {
       
                    windController.direction = -transform.right;
                    windController.power = force;
                }

                //TODO waves?
                if (powerType == "Wave")
                {
                
                    if (pitch != 0 && !float.IsNaN(pitch))
                    {
                        accu += pitch;
                        n += 1;
                    }



                }

                //Ammo?
            }
            else
            {

                cloudSystem.enableEmission = false;
            }










            //if the boat speeds up the camera moves further away and as is slows down the camera gets closer.
            if (boat.velocity.magnitude > cameraSpeedThreshold && boatCamera.distance < (cameraDistanceSpeedUp))
            {
                boatCamera.distance = Mathf.Lerp(boatCamera.distance, 100f, Time.deltaTime / 10);
                boatCamera.height = Mathf.Lerp(boatCamera.height, cameraHeightInit + 5, Time.deltaTime);
            }
            else if (boat.velocity.magnitude <= cameraSpeedThreshold && boatCamera.distance != cameraDistanceInit)
            {
                boatCamera.distance = Mathf.Lerp(boatCamera.distance, cameraDistanceInit, Time.deltaTime);
                boatCamera.height = Mathf.Lerp(boatCamera.height, cameraHeightInit, Time.deltaTime);
            }

            //boat.AddTorque(direction);

            //transform.Rotate(direction, Space.Self);


            if (Input.GetKeyDown(KeyCode.O))
            {
                development = !development;
            }
            if (development)
            {
                //if (Input.GetKey(KeyCode.W))
                //{
                //    if (boat.velocity.magnitude < 50)
                //    {
                //        boat.AddRelativeForce(-Vector3.right * 2 * windAngle);
                //    }
                //}
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


                boat.velocity *= 0.995f;

            }
            else
            {
                //gyroTest();

                if (Mathf.Abs(rotation) > 0.2f)
                {
                    transform.Rotate(Vector3.up*(rotation-0.2f));
                }
                //if (rotation > 0.2f)
                //{
                //    transform.Rotate(-Vector3.forward);
                //}
            }
        }
    }



    //void gyroTest()
    //{
    //    UnityEngine.Gyroscope gyro = Input.gyro;
    //    if (gyro == null) { return; }
    //    cylinder.rotation = gyro.attitude;
    //}


    void SetWaves()
    {

        pitch = accu / n;
        Debug.Log(pitch);

        if (float.IsNaN(pitch))
        {
            Debug.Log("Pitch is NaN!!!");
            pitch = 350;
        }
        pitch = 350 - pitch;
        pitch = Mathf.Clamp(pitch, 20, 300);

        waveVector.x = -transform.right.x;
        waveVector.y = -transform.right.z;
        waveVector.z = force;
        waveVector.w = pitch;
        waterShader.SetVector("_Wave1", waveVector);
        waves.Waves[0] = waveVector;


        Vector4 w2 = waves.Waves[1];
        Vector4 w3 = waves.Waves[2];
        Vector4 w4 = waves.Waves[3];

        w2.w = Mathf.Clamp(pitch / 2, 20, 300);
        w3.w = Mathf.Clamp(pitch * 2, 20, 300);
        w4.w = Mathf.Clamp(100 + pitch, 20,300);


        waterShader.SetVector("_Wave2", w2);
        waves.Waves[1] = w2;

        waterShader.SetVector("_Wave3", w3);
        waves.Waves[2] = w3;

        waterShader.SetVector("_Wave4", w4);
        waves.Waves[3] = w4;





    }


    void ResetPower()
    {
        
        powerType = null;
    }


}
