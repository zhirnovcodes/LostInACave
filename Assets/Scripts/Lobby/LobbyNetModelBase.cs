using UnityEngine;

public abstract class LobbyNetModelBase : MonoBehaviour
{
    public abstract void Connect();
    public abstract bool HasConnected(out ConnectionResultData result);

    public abstract void SendConnected();
    public abstract bool HasOpponentConnected();

    public abstract void SelectCharacter(CharacterType character);
    public abstract bool IsCharacterSelected();
    public abstract CharacterType HasSelected();
}
