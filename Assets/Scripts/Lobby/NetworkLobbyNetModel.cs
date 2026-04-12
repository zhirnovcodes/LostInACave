using Newtonsoft.Json.Linq;
using UnityEngine;

public class NetworkLobbyNetModel : LobbyNetModelBase
{
    public NetworkBridge Bridge;

    private bool HasReceivedResponse;
    private ConnectionResultData ConnectionResult;

    private bool OpponentConnected;
    private bool OpponentHasSelected;
    private CharacterType OpponentCharacter;

    public override void Connect()
    {
        Bridge.OnConnectResponse += OnConnectResponse;
        Bridge.OnMessageReceived += OnMessageReceived;
        Bridge.Connect();
    }

    public override bool HasConnected(out ConnectionResultData result)
    {
        result = ConnectionResult;
        return HasReceivedResponse;
    }

    public override void SelectCharacter(CharacterType character)
    {
        UnityEngine.Debug.Log($"[NetworkLobbyNetModel] Selecting character: {character}");
        var payload = NetworkHelpers.FromCharacterSelected(new NetworkHelpers.CharacterSelectedNetMessage
        {
            Type = character
        });

        Bridge.SendMessage(JObject.Parse(payload));
    }

    public override bool IsCharacterSelected()
    {
        return OpponentHasSelected;
    }

    public override CharacterType HasSelected()
    {
        return OpponentCharacter;
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
        UnityEngine.Debug.Log("[NetworkLobbyNetModel] OnMessageReceived - " + rawJson);

        var messageType = NetworkHelpers.GetType(rawJson, out var content);

        if (messageType == NetworkHelpers.MessageType.CharacterSelected)
        {
            var message = NetworkHelpers.ToCharacterSelected(content);
            OpponentCharacter = message.Type;
            OpponentHasSelected = true;
        }

        if (messageType == NetworkHelpers.MessageType.Connected)
        {
            OpponentConnected = true;
        }
    }

    public override bool HasOpponentConnected()
    {
        return OpponentConnected;
    }

    public override void SendConnected()
    {
        var payload = NetworkHelpers.FromConnected(new NetworkHelpers.ConnectedNetMessage
        {
        });

        Bridge.SendMessage(JObject.Parse(payload));
    }
}

