using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PhoneSettings", menuName = "Phone/PhoneSettings")]
public class PhoneSettings : ScriptableObject
{
    [Header("Battery Drain")]
    public float PassiveBatteryDrain;
    public float FlashLightBatteryDrain;
    public float MessageSendBatteryDrain;
    public float MessageRecievedBatteryDrain;
    public float MessagePerSymbolDrain;

    [Header("Messaging")]
    public float MessageSendInterval;

    [Header("Keys")]
    public Key WriteNewMessageKey;
    public Key SendMessageKey;
    public Key QuitWritingKey;
    public Key FlashlightKey;

    [Header("Totorial")]
    public string IntroductionMessage;
}
