using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class WilliwawNetworkManager : NetworkManager
{
    readonly Vector3[] spawnPoints = { new Vector3(720, 10, 780), new Vector3(500, 10, 300), };

    Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan };

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoints[Mathf.RoundToInt(Random.value)], Quaternion.identity);
        player.GetComponent<MapMarker>().color = colors.Last();
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        colors = colors.Take(colors.Count() - 1).ToArray();
    }
}
