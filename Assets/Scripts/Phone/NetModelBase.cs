using System.Collections.Generic;
using UnityEngine;

public abstract class NetModelBase : MonoBehaviour
{
    public static NetModelBase Instance;

    // --- Phone / Game Scene ---

    public abstract void SendSceneStarted();
    public abstract bool HasOpponentSceneStarted();

    public abstract void SendMessage(PhoneMessage message);
    public abstract bool ReceivedMessages(List<PhoneMessage> messages);

    public abstract void SendDead();
    public abstract bool IsDeadReceived();

    public abstract void SendWon();
    public abstract bool IsWonReceived();

    // --- Lobby ---

    public abstract void Connect();
    public abstract bool HasConnected(out ConnectionResultData result);

    public abstract void SendConnected();
    public abstract bool HasOpponentConnected();

    public abstract void SelectCharacter(CharacterType character);
    public abstract bool IsCharacterSelected();
    public abstract CharacterType HasSelected();
}
