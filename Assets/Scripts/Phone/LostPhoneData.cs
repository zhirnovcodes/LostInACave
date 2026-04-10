using System;
using System.Collections.Generic;

public class LostPhoneData
{
    public bool IsPhoneOn;
    public bool IsFlashLightOn;
    public float BatteryValue;    // 0–100
    public int NetworkQuality;    // 0–4
    public List<PhoneMessage> NewMessagesBuffer;
    public bool HasUnreadMessages;
    public bool IsTypingEnabled;

    public LostPhoneData()
    {
        NewMessagesBuffer = new List<PhoneMessage>();
    }
}
