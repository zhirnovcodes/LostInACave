using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class NetworkHelpers
{
    public enum LostGameState
    {
        IsDead,
        IsWon
    }

    public enum MessageType
    {
        Connected,
        SceneStarted,
        CharacterSelected,
        LostGameState,
        PhoneMessage
    }

    public struct ConnectedNetMessage { }

    public struct SceneStartedNetMessage { }

    public struct CharacterSelectedNetMessage
    {
        public CharacterType Type;
    }

    public struct LostStateNetMessage
    {
        public LostGameState State;
    }

    public struct PhoneNetMessage
    {
        public SenderType Sender;
        public string Message;

        public PhoneMessage ToPhoneMessage()
        {
            return new PhoneMessage
            {
                SenderType = Sender,
                Message = Message
            };
        }
    }

    // ------------------------------------------------
    // Deserialize
    // ------------------------------------------------

    public static MessageType GetType(string payload, out string content)
    {
        var obj = JObject.Parse(payload);
        var messageType = obj["messageType"].ToObject<MessageType>();
        content = obj["value"].ToString(Formatting.None);
        return messageType;
    }

    public static ConnectedNetMessage ToConnected(string content)
    {
        return JsonConvert.DeserializeObject<ConnectedNetMessage>(content);
    }

    public static SceneStartedNetMessage ToSceneStarted(string content)
    {
        return JsonConvert.DeserializeObject<SceneStartedNetMessage>(content);
    }

    public static CharacterSelectedNetMessage ToCharacterSelected(string content)
    {
        return JsonConvert.DeserializeObject<CharacterSelectedNetMessage>(content);
    }

    public static LostStateNetMessage ToLostState(string content)
    {
        return JsonConvert.DeserializeObject<LostStateNetMessage>(content);
    }

    public static PhoneNetMessage ToPhoneMessage(string content)
    {
        return JsonConvert.DeserializeObject<PhoneNetMessage>(content);
    }

    // ------------------------------------------------
    // Serialize
    // ------------------------------------------------

    public static string FromConnected(ConnectedNetMessage message)
    {
        return BuildPayload(MessageType.Connected, message);
    }

    public static string FromSceneStarted(SceneStartedNetMessage message)
    {
        return BuildPayload(MessageType.SceneStarted, message);
    }

    public static string FromCharacterSelected(CharacterSelectedNetMessage message)
    {
        return BuildPayload(MessageType.CharacterSelected, message);
    }

    public static string FromLostState(LostStateNetMessage message)
    {
        return BuildPayload(MessageType.LostGameState, message);
    }

    public static string FromPhoneMessage(PhoneNetMessage message)
    {
        return BuildPayload(MessageType.PhoneMessage, message);
    }

    // ------------------------------------------------
    // Internal
    // ------------------------------------------------

    private static string BuildPayload(MessageType messageType, object value)
    {
        return JsonConvert.SerializeObject(new
        {
            messageType = messageType.ToString(),
            value
        });
    }
}
