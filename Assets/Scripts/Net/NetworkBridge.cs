using System;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json;

public class NetworkBridge : MonoBehaviour
{
    [Header("Server")]
    [SerializeField] private string ServerUrl = "ws://localhost:8080";
    [SerializeField] private string sessionId = "room1";

    // Events
    public event Action<bool, bool> OnConnectResponse;  // isSuccess, isConnectedFirst
    public event Action<string> OnMessageReceived;       // raw json body

    private WebSocket Ws;
    private bool IsConnected = false;

    // ------------------------------------------------
    // Public API
    // ------------------------------------------------

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public async void Connect()
    {
        Ws = new WebSocket(ServerUrl);

        Ws.OnOpen += OnOpen;
        Ws.OnMessage += OnMessage;
        Ws.OnError += OnError;
        Ws.OnClose += OnClose;

        await Ws.Connect();
    }

    public async void SendConnectRequest()
    {
        if (!IsConnected) return;

        var packet = new
        {
            type = "CONNECT",
            sessionId = sessionId
        };

        await Ws.SendText(JsonConvert.SerializeObject(packet));
    }

    public async void SendMessage(object body)
    { 
        if (!IsConnected) return;

        Debug.Log($"[NetworkBridge] SendMessage {body}");

        var packet = new
        {
            type = "MESSAGE",
            body = body
        };

        await Ws.SendText(JsonConvert.SerializeObject(packet));
    }

    // ------------------------------------------------
    // WebSocket Callbacks
    // ------------------------------------------------

    private void OnOpen()
    {
        Debug.Log("[NetworkBridge] Connected to server");
        IsConnected = true;
        SendConnectRequest();  // Auto-send connect request on open
    }

    private void OnMessage(byte[] bytes)
    {
        var json = System.Text.Encoding.UTF8.GetString(bytes);
        var packet = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        if (!packet.ContainsKey("type"))
        {
            // Server relays packet.body directly no type wrapper
            Debug.Log($"[NetworkBridge] Relayed message: {json}");
            OnMessageReceived?.Invoke(json);
            return;
        }

        var type = packet["type"].ToString();

        if (type == "CONNECT_RESPONSE")
        {
            bool isSuccess = bool.Parse(packet["isSuccess"].ToString());
            bool isConnectedFirst = bool.Parse(packet["isConnectedFirst"].ToString());

            Debug.Log($"[NetworkBridge] Connect response � isSuccess: {isSuccess}, isConnectedFirst: {isConnectedFirst}");
            OnConnectResponse?.Invoke(isSuccess, isConnectedFirst);
        }
        else
        {
            Debug.Log($"[NetworkBridge] Message: {json}");
            OnMessageReceived?.Invoke(json);
        }
    }

    private void OnError(string error)
    {
        Debug.LogError($"[NetworkBridge] Error: {error}");
    }

    private void OnClose(WebSocketCloseCode code)
    {
        Debug.Log($"[NetworkBridge] Closed: {code}");
        IsConnected = false;
    }

    // ------------------------------------------------
    // Unity Lifecycle
    // ------------------------------------------------

    void Update()
    {
        // Required by NativeWebSocket to dispatch messages on main thread
#if !UNITY_WEBGL || UNITY_EDITOR
        Ws?.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        if (Ws != null && Ws.State == WebSocketState.Open)
        {
            await Ws.Close();
        }
    }
}