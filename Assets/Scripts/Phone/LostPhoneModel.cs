using System;
using System.Collections.Generic;

public class LostPhoneModel
{
    private LostPhoneData Data;

    public LostPhoneModel()
    {
        Data = new LostPhoneData();
    }

    public void TurnOff()
    {
        Data.IsPhoneOn = false;
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

    public void SetNetworkLevel(int value)
    {
        Data.NetworkQuality = value;
    }

    public void AddToMessagesBuffer(PhoneMessage message)
    {
        Data.NewMessagesBuffer.Add(message);
    }

    public bool DrainMessagesBuffer(List<PhoneMessage> resultMessages)
    {
        foreach (PhoneMessage msg in Data.NewMessagesBuffer)
        {
            resultMessages.Add(msg);
        }

        Data.NewMessagesBuffer.Clear();

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
}
