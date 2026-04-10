using System;
using System.Collections.Generic;

[System.Serializable]
public struct LostPhoneData
{
    public bool IsPhoneOn;
    public bool IsControlEnabled;
    public bool IsFlashLightOn;
    public float BatteryValue;    // 0–100
    public int NetworkQuality;    // 0–4
    public bool HasUnreadMessages;
    public bool IsTypingEnabled;
}
