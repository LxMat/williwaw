using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiWindDirectionController : MonoBehaviour
{
    [System.Obsolete]
    private WindController windController;
    private Vector3 direction;
    private Image arrow;
    private Image boatIcon;
    private Vector3 rotationDiff;
    private Vector3 diff = new Vector3(0,0,0);

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        windController = GameObject.Find("Wind(Clone)").GetComponent<WindController>();
        arrow = transform.Find("WindDirIcon").GetComponent<Image>();
        boatIcon = transform.Find("BoatIcon").GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        direction = windController.direction;
        rotationDiff = (Quaternion.LookRotation(direction).eulerAngles - transform.parent.rotation.eulerAngles);
        diff.z = (-45 + (transform.parent.rotation.eulerAngles.y - Quaternion.LookRotation(direction).eulerAngles.z)) % 360; //Don't know why we there is a 45 degree offset but there is...
        arrow.transform.rotation = Quaternion.Euler(diff);
        arrow.transform.position = boatIcon.transform.position; //reset previous position
        arrow.transform.Translate(new Vector3(-65, 0, 0)); 

    }
}
