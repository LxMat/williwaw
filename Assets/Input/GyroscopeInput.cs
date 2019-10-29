using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroscopeInput : MonoBehaviour
{
    // Start is called before the first frame update
    private Gyroscope m_Gyro;
    private Compass m_Comp;
   
    public float rotation;

    private float shakeThreshold = 0.1f;
    public float shakeAmount;

    private Vector3 north;
    private Vector3 down;


    public float smooth = 0.2f;
    public Text inputValue;
    public float newRotation;
    public float sensitivity = 3;
    private Vector3 currentAcceleration, initialAcceleration;



    private float accu = 0.0f;
    private int n = 0;




    private void Start()
    {
        //Set up and enable the gyroscope (check your device has one)
        m_Gyro = Input.gyro;
        m_Comp = Input.compass;


        initialAcceleration = Input.acceleration;
        currentAcceleration = Vector3.zero;

        m_Comp.enabled = true;
        m_Gyro.enabled = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //get north right now
        north = new Vector3(Mathf.Cos(m_Comp.magneticHeading), 0f, Mathf.Sin(m_Comp.magneticHeading));
        //get down right now
        down = m_Gyro.gravity;



        //This is a legacy function, check out the UI section for other ways to create your UI
    }
    private void Update()
    {
        //Quaternion.LookRotation(m_Gyro.gravity, new Vector3(Mathf.Cos(m_Comp.magneticHeading), 0f, Mathf.Sin(m_Comp.magneticHeading))) * Quaternion.FromToRotation(Vector3.forward, Vector3.up);
        //north = new Vector3(Mathf.Cos(m_Comp.magneticHeading), 0f, Mathf.Sin(m_Comp.magneticHeading));
        //transform.rotation = Quaternion.LookRotation(north, -m_Gyro.gravity);
        //rotation = m_Gyro.attitude.y;//Mathf.Round(m_Comp.magneticHeading * Mathf.Deg2Rad * 100)/100;

        currentAcceleration = Vector3.Lerp(currentAcceleration, Input.acceleration - initialAcceleration, Time.deltaTime / smooth);

        newRotation = Mathf.Clamp(currentAcceleration.x * sensitivity, -1, 1);
        //transform.Rotate(0, 0, -newRotation);
        rotation = newRotation;

        //while (Time.time < 5.0f)
        //{
        //    accu = 
        //}


        if (m_Gyro.userAcceleration.x >= shakeThreshold)
        {
            shakeAmount += m_Gyro.userAcceleration.x * Time.deltaTime;
        }
        shakeAmount = shakeAmount - shakeAmount * 0.01f * Time.deltaTime;
        if (shakeAmount > 1.0f)
        {
            shakeAmount = 1.0f;
        }
        if (shakeAmount <= 0.1f)
        {
            shakeAmount = 0.1f;
        }
    }

    private void OnGUI()
    {
        // GUI.skin.label.fontSize = 28;
        // GUI.contentColor = Color.red;
        //GUI.Label(new Rect(500, 250, 300, 40), "Compass "+ Quaternion.Euler(0, -Input.compass.magneticHeading, 0));
        ////Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
        //GUI.Label(new Rect(500, 300, 300, 40), "Gyro rotation rate " + m_Gyro.rotationRate);
        //GUI.Label(new Rect(500, 350, 300, 40), "Acceleration" + m_Gyro.userAcceleration.x);
        //GUI.Label(new Rect(500, 400, 300, 40), "Rotation : " + rotation);
    }
}
