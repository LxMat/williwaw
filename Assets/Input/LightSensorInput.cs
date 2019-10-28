using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSensorInput : MonoBehaviour
{

    public float lux;
    public bool spawnCloud;
    public float threshold = 40.0f;
    private float timer = 0.0f;
    private float waitTime = 5.0f;
    private float accu = 0.0f;
    private int n = 0;
    // Start is called before the first frame update
    void Start()
    {
        //var lightSensor = new LightSensor();
        //InputSystem.AddDevice(lightSensor);
        if(Application.platform == RuntimePlatform.Android)
        {
            InputSystem.EnableDevice(LightSensor.current);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (LightSensor.current != null)
        {
            lux = LightSensor.current.lightLevel.ReadValue();
            timer += Time.deltaTime;
            if (timer < waitTime)
            {
                accu += lux;
                n += 1;

                threshold = accu / (2 * n);


            }
            else
            {
                if (lux < threshold)
                {
                    spawnCloud = true;
                }
                else
                {
                    spawnCloud = false;
                }
            }
        }

    }

    void OnGUI()
    {
        GUI.Label(new Rect(600, 500, 300, 40), "LUX: " + lux);
        GUI.Label(new Rect(700, 500, 300, 40), "Threshold: " + threshold);
    }
}
