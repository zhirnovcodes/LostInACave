using UnityEngine;
using UnityEngine.UI;

public class OperatorPhoneUIView : MonoBehaviour
{
    [Header("HUD")]
    public Image NewMessageImage;

    [Header("Message")]
    public DialogueBoxElement DialogueBoxElement;
    public MessageBoxElement MessageBoxElement;
    public GameObject HasNetworkGroup;
    public GameObject WaitingGroup;
    public GameObject InstructionsGroup;
}
