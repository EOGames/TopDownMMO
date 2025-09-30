using System;
using UnityEngine;
using SocketIOClient;

public class SocketManager : MonoBehaviour
{
    public static SocketManager Instance { get; private set; }
    private SocketIOUnity mySocket;

    public string serverUrl = "https://www.example.com";
    bool isConnected = false;

    //Handlers
    [SerializeField] private SpawnHandler spawnHandler;
    [SerializeField] MovementHandler movementHandler;
    [SerializeField] private WeaponHandler wpnHandler;
    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CreateSocket();
    }

    void CreateSocket()
    {
        print("Creating Socket");
        var uri = new Uri(serverUrl);

        mySocket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Reconnection = true,              // enable auto-reconnect
            ReconnectionAttempts = 5,         // try 5 times
            ReconnectionDelay = 2000          // 2 seconds delay between tries
        });

        // Subscribe to core events
        mySocket.OnConnected += (sender, e) =>
        {
            isConnected = true;
            Debug.Log("✅ Connected to server");

            RegisterAllHandlers();
        };
       
        // 🔄 Reconnect attempt
        mySocket.OnReconnectAttempt += (sender, attempt) =>
        {
            Debug.Log($"🔄 Reconnect attempt: {attempt}");
        };

        // When connection fails
        mySocket.OnError += (sender, e) =>
        {
            Debug.LogError("❌ Socket error: " + e);
        };

        // When cannot connect (server down)
        mySocket.OnReconnectFailed += (sender, e) =>
        {
            Debug.LogError("❌ Failed to reconnect after max attempts!");
        };

        mySocket.OnDisconnected += (sender, e) =>
        {
            isConnected = false;
            Debug.Log("❌ Disconnected from server");
        };
        print("Trying to connect to server");

        mySocket.On("playerDisconnected", (SocketIOResponse resp) =>
        {
            string disconnectingId = resp.GetValue().ToString();
            print(("Disconnecting from server Player ID "+disconnectingId));
            GameManager.Instance.RemovePlayer(disconnectingId);
        });
        mySocket.Connect();
    }

    void RegisterAllHandlers()
    {
        spawnHandler.RegisterHandler(mySocket);
        movementHandler.RegisterHandler(mySocket);
        wpnHandler.RegisterSocket(mySocket);
    }

    public SpawnHandler GetSpawnHandler()
    {
        return spawnHandler;
    }

    public MovementHandler GetMovementHandler()
    {
        return movementHandler;
    }

    public WeaponHandler GetWeaponHandler()
    {
        return wpnHandler;
    }
    void OnDestroy()
    {
        mySocket.Disconnect();
    }
}

public class PlayerInfo
{
    public string id;
    public Vector3 spawnPosition;
    public int selectedCharacter;
    public string characterName;
}




