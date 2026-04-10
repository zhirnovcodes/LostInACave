using System.Collections.Generic;
using UnityEngine;

public class OperatorPhoneModel : MonoBehaviour
{
    [SerializeField] private OperatorPhoneData Data;

    private float LastMessageSendTime = float.NegativeInfinity;

    public void DisableControl()
    {
        Data.IsControlEnabled = false;
    }

    public bool IsControlEnabled()
    {
        return Data.IsControlEnabled;
    }

    public void RecordMessageSent()
    {
        LastMessageSendTime = Time.time;
    }

    public bool IsWaitingMessage(float interval)
    {
        return Time.time - LastMessageSendTime < interval;
    }

    public void SetNewMessagesRead()
    {
        Data.HasUnreadMessages = false;
    }

    public void SetHasNewMessages()
    {
        Data.HasUnreadMessages = true;
    }

    public void ToggleTypingEnabled()
    {
        Data.IsTypingEnabled = !Data.IsTypingEnabled;
    }

    public bool IsTypingEnabled()
    {
        return Data.IsTypingEnabled;
    }

    public bool HasUnreadMessages()
    {
        return Data.HasUnreadMessages;
    }
}
