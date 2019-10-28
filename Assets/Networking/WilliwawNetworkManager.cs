using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class WilliwawNetworkManager : NetworkManager
{

    Vector3[] spawnPoints = { new Vector3(720, 10, 780), new Vector3(500, 10, 300), };

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, spawnPoints[Mathf.RoundToInt(Random.value)], Quaternion.identity);
        player.GetComponent<MapMarker>().color = Color.red;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
