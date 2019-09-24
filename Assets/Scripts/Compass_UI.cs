using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass_UI : MonoBehaviour
{
    public Vector3 NorthDirection;
    public Transform Player;

    public RectTransform Northlayer;

    // Update is called once per frame
    void Update()
    {
        ChangeNorthDirection(); 
    }

    public void ChangeNorthDirection()
    {
        NorthDirection.z = Player.eulerAngles.y;
        Northlayer.localEulerAngles = NorthDirection;
    }

}
