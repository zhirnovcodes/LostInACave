using TMPro;
using UnityEngine;

public class MessageBoxElement : MonoBehaviour
{
    public TMP_InputField RichTextField;

    public void SetTypingEnabled()
    {
        if (RichTextField.interactable == false)
        {
            RichTextField.interactable = true;
            RichTextField.ActivateInputField();
        }
    }

    public void Activate()
    {
        RichTextField.ActivateInputField();
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
