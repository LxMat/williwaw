using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

[System.Obsolete]
public class MapMarker : NetworkBehaviour
{
    public GameObject Marker;
    public Color color;

    // Update is called once per frame

    public override void OnStartLocalPlayer()
    {
        Marker.SetActive(false);
    }

    private void Start()
    {
        Material material = new Material(Shader.Find("Standard"))
        {
            color = color
        };
        Marker.GetComponent<MeshRenderer>().material = material;
    }
}
