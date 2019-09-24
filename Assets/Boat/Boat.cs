using UnityEngine;
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
    private bool development;
    private WaterPlane waves;

    // Start is called before the first frame update
    private void Start()
    {
        boat = GetComponent<Rigidbody>();
        direction = new Vector3();
        development = false;

    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<SmoothFollow>().target = follow.transform;
    }

    //Awake is called after all objects are initialized.
    void Awake()
    {
        waves = GameObject.Find("Waves").GetComponent<WaterPlane>();
    }


    // Update is called once per frame
    private void Update()
    {
        if (isLocalPlayer)
        {

            Vector3 currentRot = transform.rotation.eulerAngles;
            Vector3 currentPos = transform.position;
            Vector3 newPos = new Vector3(currentPos.x, waves.getHeight(currentPos), currentPos.z);
            transform.position = newPos;
            transform.rotation = Quaternion.Euler(new Vector3(-90, currentRot.y, currentRot.z)); // stops boat from flipping around.


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
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Rotate(-Vector3.forward);
                }
                if (Input.GetKey(KeyCode.D))
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
                if (rotation < -0.2f)
                {
                    transform.Rotate(-Vector3.forward);
                }
                if (rotation > 0.2f)
                {
                    transform.Rotate(Vector3.forward);
                }
            }
        }
    }
}
