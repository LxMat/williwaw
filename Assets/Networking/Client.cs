using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private const int MAX_USERS = 100;
    private const int PORT = 8888;
    private const string SERVER_IP = "127.0.0.1";
    private const int BYTE_SIZE = 1024;


    private byte reliableChannel;
    private byte unreliableChannel;
    private int connectionId;
    private int hostId;
    private bool isStarted = false;
    private byte error;

    // Start is called before the first frame update
    [System.Obsolete]
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    [System.Obsolete]
    public void Init()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        unreliableChannel = cc.AddChannel(QosType.UnreliableSequenced);
        reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USERS);


        // Client only
        hostId = NetworkTransport.AddHost(topo, 0);
        // Native
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, PORT, 0, out error);
        Debug.Log("Connecting from Native");

        Debug.Log(string.Format("Attempting to connect on {0}...", SERVER_IP));
        isStarted = true;
    }

    // Update is called once per frame
    [System.Obsolete]
    private void Update()
    {
        MessageHandler();
    }

    [System.Obsolete]
    public void Shutdown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }

    [System.Obsolete]
    private void MessageHandler()
    {
        if (!isStarted) { return; }

        byte[] recBuffer = new byte[BYTE_SIZE];

        NetworkEventType messageType = NetworkTransport.Receive(out int recHostId, out int connectionId, out int channelId, recBuffer, BYTE_SIZE, out int dataSize, out error);

        switch (messageType)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("You have connected to the server");
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("You have disconnected rom the server");
                break;
            case NetworkEventType.DataEvent:
                Debug.Log("Data!");
                break;
            case NetworkEventType.BroadcastEvent:
            default:
                Debug.Log("Unexpected network event");
                break;

        }
    }

    [System.Obsolete]
    public void SendToServer()
    {
        byte[] buffer = new byte[BYTE_SIZE];

        // Create message here
        buffer[0] = 1;

        // Send message
        NetworkTransport.Send(hostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);
    }
}
