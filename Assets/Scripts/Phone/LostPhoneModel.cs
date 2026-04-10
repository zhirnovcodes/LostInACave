using System.Collections.Generic;
using UnityEngine;

public class LostPhoneModel : MonoBehaviour
{
    [SerializeField] private LostPhoneData Data;
    public List<PhoneMessage> NewMessagesBuffer = new List<PhoneMessage>();

    private float LastMessageSendTime = float.NegativeInfinity;

    private void Awake()
    {
        //Data = new LostPhoneData();
    }

    public void TurnOff()
    {
        Data.IsPhoneOn = false;
    }

    public void DisableControl()
    {
        Data.IsControlEnabled = false;
    }

    public bool IsControlEnabled()
    {
        return Data.IsControlEnabled;
    }

    public void ToggleFlashLight()
    {
        Data.IsFlashLightOn = !Data.IsFlashLightOn;
    }

    public bool IsFlashOn => Data.IsFlashLightOn;

    public float GetBatteryLevel()
    {
        return Data.BatteryValue;
    }

    public void SetBatteryLevel(float value)
    {
        Data.BatteryValue = value;
    }

    public int GetNetworkLevel()
    {
        return Data.NetworkQuality;
    }

    public void RecordMessageSent()
    {
        LastMessageSendTime = Time.time;
    }

    public bool IsWaitingMessage(float interval)
    {
        return Time.time - LastMessageSendTime < interval;
    }

    public void SetNetworkLevel(int value)
    {
        Data.NetworkQuality = value;
    }

    public void AddToMessagesBuffer(PhoneMessage message)
    {
        NewMessagesBuffer.Add(message);
    }

    public bool DrainMessagesBuffer(List<PhoneMessage> resultMessages)
    {
        foreach (PhoneMessage msg in NewMessagesBuffer)
        {
            resultMessages.Add(msg);
        }

        NewMessagesBuffer.Clear();

        if (resultMessages.Count > 0)
        {
            Data.HasUnreadMessages = true;
            return true;
        }

        return false;
    }

    public void SetNewMessagesRead()
    {
        Data.HasUnreadMessages = false;
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
