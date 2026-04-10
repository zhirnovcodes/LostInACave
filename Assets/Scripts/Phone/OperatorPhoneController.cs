using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OperatorPhoneController : MonoBehaviour
{
    public OperatorPhoneUIView UIView;
    public PhoneSettings Settings;
    public NetModelBase NetModel;
    public OperatorPhoneModel PhoneModel;

    private List<PhoneMessage> MessageBuffer;

    private void Awake()
    {
        MessageBuffer = new List<PhoneMessage>();
    }

    private void Update()
    {
        UpdateHUD();
        UpdateMessages();
        UpdateDialogue();
        UpdateTextBox();
    }

    private void UpdateHUD()
    {
        UIView.NewMessageImage.enabled = PhoneModel.HasUnreadMessages();

        if (PhoneModel.HasUnreadMessages() && UIView.DialogueBoxElement.IsAtBottom())
        {
            PhoneModel.SetNewMessagesRead();
        }
    }

    private void UpdateMessages()
    {
        if (NetModel.ReceivedMessages(MessageBuffer))
        {
            foreach (PhoneMessage message in MessageBuffer)
            {
                UIView.DialogueBoxElement.AddMessage(message);
                PhoneModel.SetHasNewMessages();
            }
        }
    }

    private void UpdateDialogue()
    {
        if (PhoneModel.IsTypingEnabled())
        {
            return;
        }

        if (HadControl(Settings.WriteNewMessageKey))
        {
            PhoneModel.ToggleTypingEnabled();
        }

        UpdateMessageLabels();

        if (PhoneModel.IsTypingEnabled())
        {
            UIView.DialogueBoxElement.SetScrollingDisabled();
            return;
        }

        UIView.DialogueBoxElement.SetScrollingEnabled();

        if (UIView.DialogueBoxElement.IsAtBottom())
        {
            PhoneModel.SetNewMessagesRead();
        }

        MessageBuffer.Clear();
    }

    private void UpdateTextBox()
    {
        if (PhoneModel.IsTypingEnabled() == false)
        {
            UIView.MessageBoxElement.SetTypingDisabled();
            return;
        }

        UIView.MessageBoxElement.SetTypingEnabled();

        UpdateMessageLabels();

        var canSendMessage = PhoneModel.IsWaitingMessage(Settings.MessageSendInterval) == false;
        var sendKeyPressed = HadControl(Settings.SendMessageKey);

        if (canSendMessage && sendKeyPressed)
        {
            string text = UIView.MessageBoxElement.GetText();
            var message = new PhoneMessage { SenderType = SenderType.Operator, Message = text };

            NetModel.SendMessage(message);
            PhoneModel.RecordMessageSent();

            UIView.MessageBoxElement.Clear();
            UIView.MessageBoxElement.Activate();

            UIView.DialogueBoxElement.AddMessage(message);
            UIView.DialogueBoxElement.ScrollToBottom();
            return;
        }

        if (HadControl(Settings.QuitWritingKey))
        {
            PhoneModel.ToggleTypingEnabled();
            UpdateMessageLabels();
        }
    }

    private void UpdateMessageLabels()
    {
        if (PhoneModel.IsTypingEnabled() == false)
        {
            UIView.HasNetworkGroup.SetActive(false);
            UIView.WaitingGroup.SetActive(false);
            UIView.InstructionsGroup.SetActive(true);
            return;
        }

        if (PhoneModel.IsWaitingMessage(Settings.MessageSendInterval))
        {
            UIView.HasNetworkGroup.SetActive(false);
            UIView.WaitingGroup.SetActive(true);
            UIView.InstructionsGroup.SetActive(false);
            return;
        }

        UIView.HasNetworkGroup.SetActive(true);
        UIView.WaitingGroup.SetActive(false);
        UIView.InstructionsGroup.SetActive(false);
    }

    private bool HadControl(Key key)
    {
        if (PhoneModel.IsControlEnabled())
        {
            return Keyboard.current[key].wasPressedThisFrame;
        }

        return false;
    }
}
