using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SocketNetModel : NetModelBase
{
    // --- Phone / Game Scene ---

    private bool OpponentSceneStarted;
    private List<PhoneMessage> PendingMessages = new List<PhoneMessage>();
    private bool DeadReceived;
    private bool WonReceived;

    // --- Lobby ---

    private bool HasReceivedResponse;
    private ConnectionResultData ConnectionResult;
    private bool OpponentConnected;
    private bool OpponentHasSelected;
    private CharacterType OpponentCharacter;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        NetworkBridge.Instance.OnMessageReceived += OnMessageReceived;
    }

    // --- Phone / Game Scene ---

    public override void SendSceneStarted()
    {
        var payload = NetworkHelpers.FromSceneStarted(new NetworkHelpers.SceneStartedNetMessage());
        NetworkBridge.Instance.SendMessage(JObject.Parse(payload));
    }

    public override bool HasOpponentSceneStarted()
    {
        return OpponentSceneStarted;
    }

    public override void SendMessage(PhoneMessage message)
    {
        var payload = NetworkHelpers.FromPhoneMessage(new NetworkHelpers.PhoneNetMessage
        {
            Sender = message.SenderType,
            Message = message.Message
        });

        NetworkBridge.Instance.SendMessage(JObject.Parse(payload));
    }

    public override bool ReceivedMessages(List<PhoneMessage> messages)
    {
        if (PendingMessages.Count == 0)
        {
            return false;
        }

        foreach (PhoneMessage message in PendingMessages)
        {
            messages.Add(message);
        }

        PendingMessages.Clear();
        return true;
    }

    public override void SendDead()
    {
        var payload = NetworkHelpers.FromLostState(new NetworkHelpers.LostStateNetMessage
        {
            State = NetworkHelpers.LostGameState.IsDead
        });

        NetworkBridge.Instance.SendMessage(JObject.Parse(payload));
    }

    public override bool IsDeadReceived()
    {
        return DeadReceived;
    }

    public override void SendWon()
    {
        var payload = NetworkHelpers.FromLostState(new NetworkHelpers.LostStateNetMessage
        {
            State = NetworkHelpers.LostGameState.IsWon
        });

        NetworkBridge.Instance.SendMessage(JObject.Parse(payload));
    }

    public override bool IsWonReceived()
    {
        return WonReceived;
    }

    // --- Lobby ---

    public override void Connect()
    {
        NetworkBridge.Instance.OnConnectResponse += OnConnectResponse;
        NetworkBridge.Instance.Connect();
    }

    public override bool HasConnected(out ConnectionResultData result)
    {
        result = ConnectionResult;
        return HasReceivedResponse;
    }

    public override void SelectCharacter(CharacterType character)
    {
        Debug.Log($"[SocketNetModel] Selecting character: {character}");
        var payload = NetworkHelpers.FromCharacterSelected(new NetworkHelpers.CharacterSelectedNetMessage
        {
            Type = character
        });

        NetworkBridge.Instance.SendMessage(JObject.Parse(payload));
    }

    public override bool IsCharacterSelected()
    {
        return OpponentHasSelected;
    }

    public override CharacterType HasSelected()
    {
        return OpponentCharacter;
    }

    public override bool HasOpponentConnected()
    {
        return OpponentConnected;
    }

    public override void SendConnected()
    {
        var payload = NetworkHelpers.FromConnected(new NetworkHelpers.ConnectedNetMessage());
        NetworkBridge.Instance.SendMessage(JObject.Parse(payload));
    }

    private void OnConnectResponse(bool isSuccess, bool isConnectedFirst)
    {
        ConnectionResult = new ConnectionResultData
        {
            IsSuccess = isSuccess,
            IsConnectedFirst = isConnectedFirst,
        };
        HasReceivedResponse = true;
    }

    private void OnMessageReceived(string rawJson)
    {
        Debug.Log("[SocketNetModel] OnMessageReceived - " + rawJson);

        var messageType = NetworkHelpers.GetType(rawJson, out var content);

        if (messageType == NetworkHelpers.MessageType.SceneStarted)
        {
            OpponentSceneStarted = true;
        }

        if (messageType == NetworkHelpers.MessageType.PhoneMessage)
        {
            var netMessage = NetworkHelpers.ToPhoneMessage(content);
            PendingMessages.Add(netMessage.ToPhoneMessage());
        }

        if (messageType == NetworkHelpers.MessageType.LostGameState)
        {
            var netMessage = NetworkHelpers.ToLostState(content);

            if (netMessage.State == NetworkHelpers.LostGameState.IsDead)
            {
                DeadReceived = true;
            }

            if (netMessage.State == NetworkHelpers.LostGameState.IsWon)
            {
                WonReceived = true;
            }
        }

        if (messageType == NetworkHelpers.MessageType.CharacterSelected)
        {
            var netMessage = NetworkHelpers.ToCharacterSelected(content);
            OpponentCharacter = netMessage.Type;
            OpponentHasSelected = true;
        }

        if (messageType == NetworkHelpers.MessageType.Connected)
        {
            OpponentConnected = true;
        }
    }
}
