using System.Collections.Generic;
using UnityEngine;

public abstract class NetModelBase : MonoBehaviour
{
    public abstract void SendConnected();
    public abstract bool HasOpponentConnected();

    public abstract void SendMessage(PhoneMessage message);

    public abstract bool ReceivedMessages(List<PhoneMessage> messages);
    public abstract void SendDead();
}
