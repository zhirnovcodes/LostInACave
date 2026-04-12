using System.Collections.Generic;
using UnityEngine;

public abstract class NetModelBase : MonoBehaviour
{
    public abstract void SendSceneStarted();
    public abstract bool HasOpponentSceneStarted();

    public abstract void SendMessage(PhoneMessage message);

    public abstract bool ReceivedMessages(List<PhoneMessage> messages);
    public abstract void SendDead();
    public abstract bool IsDeadReceived();
    public abstract void SendWon();
    public abstract bool IsWonReceived();
}
