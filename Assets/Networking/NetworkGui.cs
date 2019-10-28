using System;
using System.ComponentModel;


namespace UnityEngine.Networking
{

    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkGui : MonoBehaviour
    {
        private NetworkManager manager;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        void OnGUI()
        {

            int xpos = 25;
            int ypos = 50;
            int buttonWidth = 400;
            int buttonHeight = 100;
            int spacing = buttonHeight + 10;

            bool noConnection = (manager.client == null || manager.client.connection == null ||
                manager.client.connection.connectionId == -1);

            if (!manager.IsClientConnected() && !NetworkServer.active)
            {
                if (noConnection)
                {
                    if (GUI.Button(new Rect(xpos, ypos, buttonWidth, buttonHeight), "LAN Host(H)"))
                    {
                        manager.StartHost();
                    }
                    ypos += spacing;

                    if (GUI.Button(new Rect(xpos, ypos, buttonWidth / 2, buttonHeight), "LAN Client(C)"))
                    {
                        manager.StartClient();
                    }
                    manager.networkAddress = GUI.TextField(new Rect(xpos + buttonWidth / 2, ypos, buttonWidth / 2, buttonHeight), manager.networkAddress);
                    ypos += spacing;

                    if (GUI.Button(new Rect(xpos, ypos, buttonWidth, buttonHeight), "LAN Server Only(S)"))
                    {
                        manager.StartServer();
                    }
                    ypos += spacing;
                }
                else
                {
                    GUI.Label(new Rect(xpos, ypos, buttonWidth, buttonHeight), "Connecting to " + manager.networkAddress + ":" + manager.networkPort + "..");
                    ypos += spacing;


                    if (GUI.Button(new Rect(xpos, ypos, buttonWidth, buttonHeight), "Cancel Connection Attempt"))
                    {
                        manager.StopClient();
                    }
                }
            }
            else
            {
                if (NetworkServer.active)
                {
                    string serverMsg = "Server: port=" + manager.networkPort;
                    if (manager.useWebSockets)
                    {
                        serverMsg += " (Using WebSockets)";
                    }
                    GUI.Label(new Rect(xpos, ypos, buttonWidth, buttonHeight), serverMsg);
                    ypos += spacing;
                }
                if (manager.IsClientConnected())
                {
                    GUI.Label(new Rect(xpos, ypos, buttonWidth, buttonHeight), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                    ypos += spacing;
                }
            }

            if (manager.IsClientConnected() && !ClientScene.ready)
            {
                if (GUI.Button(new Rect(xpos, ypos, buttonWidth, buttonHeight), "Client Ready"))
                {
                    ClientScene.Ready(manager.client.connection);

                    if (ClientScene.localPlayers.Count == 0)
                    {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }

            if (NetworkServer.active || manager.IsClientConnected())
            {
                if (GUI.Button(new Rect(xpos, ypos, buttonWidth, buttonHeight), "Quit"))
                {
                    manager.StopHost();
                }
                ypos += spacing;
            }
        }
    }
}

