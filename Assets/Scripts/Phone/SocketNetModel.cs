using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SocketNetModel : NetModelBase
{
    public NetworkBridge Bridge;

    private bool OpponentSceneStarted;

    private List<PhoneMessage> PendingMessages = new List<PhoneMessage>();

    private bool DeadReceived;
    private bool WonReceived;

    private void Awake()
    {
        GetBridge().OnMessageReceived += OnMessageReceived;
    }

    private NetworkBridge GetBridge()
    {
        return Bridge ?? NetworkBridge.Instance;
    }

    public override void SendSceneStarted()
    {
        var payload = NetworkHelpers.FromSceneStarted(new NetworkHelpers.SceneStartedNetMessage());
        GetBridge().SendMessage(JObject.Parse(payload));
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

        GetBridge().SendMessage(JObject.Parse(payload));
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

        GetBridge().SendMessage(JObject.Parse(payload));
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

        GetBridge().SendMessage(JObject.Parse(payload));
    }

    public override bool IsWonReceived()
    {
        return WonReceived;
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
    }
}
