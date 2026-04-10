using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueBoxElement : MonoBehaviour
{
    public GameObject LostMessageLabel;       // GameObject containing LostTextField
    public GameObject OperatorMessageLabel;   // GameObject containing OperatorTextField
    public TMP_Text LostTextField;
    public TMP_Text OperatorTextField;
    public Scrollbar Scrollbar;

    private List<PhoneMessage> Messages = new List<PhoneMessage>();
    private int SelectedMessageIndex;
    private bool ScrollingEnabled;

    public void SetScrollingEnabled()
    {
        ScrollingEnabled = true;
    }

    public void SetScrollingDisabled()
    {
        ScrollingEnabled = false;
    }

    public void AddMessage(PhoneMessage message)
    {
        Messages.Add(message);
        RefreshView();
    }

    public bool IsAtBottom()
    {
        return SelectedMessageIndex == Messages.Count - 1;
    }

    private void Update()
    {
        if (!ScrollingEnabled)
        {
            return;
        }

        var kb = Keyboard.current;

        if (kb.upArrowKey.wasPressedThisFrame && SelectedMessageIndex > 0)
        {
            SelectedMessageIndex--;
            RefreshView();
        }

        if (kb.downArrowKey.wasPressedThisFrame && SelectedMessageIndex < Messages.Count - 1)
        {
            SelectedMessageIndex++;
            RefreshView();
        }
    }

    private void RefreshView()
    {
        if (Messages.Count == 0)
        {
            return;
        }

        var maxIndex = Mathf.Max(1, Messages.Count - 1);
        Scrollbar.value = SelectedMessageIndex / (float)maxIndex;
        Scrollbar.value = (Scrollbar.value == 0 && Messages.Count == 1) ? 1 : Scrollbar.value;

        PhoneMessage selected = Messages[SelectedMessageIndex];
        bool isLost = selected.SenderType == SenderType.Lost;

        LostMessageLabel.SetActive(isLost);
        OperatorMessageLabel.SetActive(!isLost);

        if (isLost)
        {
            LostTextField.text = selected.ToString();
        }
        else
        {
            OperatorTextField.text = selected.ToString();
        }
    }

    public void ScrollToBottom()
    {
        SelectedMessageIndex = Messages.Count - 1;
        RefreshView();
    }
}
