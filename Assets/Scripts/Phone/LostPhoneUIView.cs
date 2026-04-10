using UnityEngine;
using UnityEngine.UI;

public class LostPhoneUIView : MonoBehaviour
{
    [Header("HUD")]
    public Image NewMessageImage;
    public BatteryElement BatteryElement;
    public NetworkElement NetworkElement;

    [Header("Message")]
    public DialogueBoxElement DialogueBoxElement;
    public MessageBoxElement MessageBoxElement;
    public GameObject HasNetworkGroup;
    public GameObject NoNetworkGroup;
}
