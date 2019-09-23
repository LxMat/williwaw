using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_USERS = 100;
    private const int PORT = 8888;
    private const int BYTE_SIZE = 1024;

    private byte reliableChannel;
    private byte unreliableChannel;
    private int hostId;
    private int webHostId;
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
    private void Init()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        unreliableChannel = cc.AddChannel(QosType.UnreliableSequenced);
        reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USERS);


        // Server only
        hostId = NetworkTransport.AddHost(topo, PORT, null);

        Debug.Log(string.Format("Opening connection on port {0}", PORT));
        isStarted = true;
    }

    // Update is called once per frame
    [System.Obsolete]
    private void Update()
    {
        MessageHandler();
    }

    [System.Obsolete]
    private void Shutdown()
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
                Debug.Log(string.Format("User {0} has connected!", connectionId));
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log(string.Format("User {0} has disconnected.", connectionId));
                break;
            case NetworkEventType.DataEvent:
                Debug.Log(recBuffer[0]);
                break;
            case NetworkEventType.BroadcastEvent:
            default:
                Debug.Log("Unexpected network event");
                break;

        }
    }
}
