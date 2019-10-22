using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

[System.Obsolete]
public class MapMarker : NetworkBehaviour
{

    public GameObject Marker;

    // Update is called once per frame

    public override void OnStartLocalPlayer()
    {
        Marker.SetActive(false);
    }

    private void Start()
    {
        Material material = new Material(Shader.Find("Standard"));
        Color color = new Color(200, 50, 50, 1);
        material.color = color;
        Marker.GetComponent<MeshRenderer>().material = material;
    }
}
