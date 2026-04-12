public class MockLobbyNetModel : LobbyNetModelBase
{
    public bool IsConnected;
    public bool IsOpponentConnected;
    public bool IsSuccess;
    public bool ConnectedFirst;
    public bool SelectedCharacter;
    public CharacterType Character;

    public override void Connect()
    {
        UnityEngine.Debug.Log("[MockLobbyNetModel] Connect");
    }

    public override bool HasConnected(out ConnectionResultData data)
    {
        data = new ConnectionResultData
        {
            IsSuccess = IsSuccess,
            IsConnectedFirst = ConnectedFirst,
        };
        return IsConnected;
    }

    public override void SelectCharacter(CharacterType character)
    {
        Character = character;
        SelectedCharacter = true;
    }

    public override bool IsCharacterSelected()
    {
        return SelectedCharacter;
    }

    public override CharacterType HasSelected()
    {
        return Character;
    }

    public override bool HasOpponentConnected()
    {
        return IsOpponentConnected;
    }

    public override void SendConnected()
    {
        UnityEngine.Debug.Log("[MockLobbyNetModel] SendConnected");
    }
}
