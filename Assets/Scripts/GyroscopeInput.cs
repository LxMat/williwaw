using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroscopeInput : MonoBehaviour
{
    // Start is called before the first frame update
    Gyroscope m_Gyro;
    Compass m_Comp;
    public float rotation;

    void Start()
    {
        //Set up and enable the gyroscope (check your device has one)
        m_Gyro = Input.gyro;
        m_Comp = Input.compass;
        m_Comp.enabled = true;
        m_Gyro.enabled = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    //This is a legacy function, check out the UI section for other ways to create your UI
    void Update()
    {
        rotation =Input.compass.magneticHeading * Mathf.Deg2Rad;
    }
    void OnGUI()
    {
        GUI.skin.label.fontSize = 20;
        GUI.contentColor = Color.red;
        GUI.Label(new Rect(500, 250, 300, 40), "Compass "+ Quaternion.Euler(0, -Input.compass.magneticHeading, 0));
        //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
        GUI.Label(new Rect(500, 300, 300, 40), "Gyro rotation rate " + m_Gyro.rotationRate);
        GUI.Label(new Rect(500, 350, 300, 40), "Gyro attitude" + m_Gyro.attitude);
        GUI.Label(new Rect(500, 400, 300, 40), "Rotation : " + rotation);
    }
}
