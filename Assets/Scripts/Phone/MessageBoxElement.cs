using TMPro;
using UnityEngine;

public class MessageBoxElement : MonoBehaviour
{
    public TMP_InputField RichTextField;

    public void SetTypingEnabled()
    {
        RichTextField.interactable = true;
    }

    public void SetTypingDisabled()
    {
        RichTextField.interactable = false;
    }

    public string GetText()
    {
        return RichTextField.text;
    }

    public void Clear()
    {
        RichTextField.text = string.Empty;
    }
}
