using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LostPhoneController : MonoBehaviour
{
    public LostPhoneUIView UIView;
    public LostPhoneSceneView SceneView;
    public PhoneSettings Settings;
    public NetModelBase NetModel;
    public LostPhoneModel PhoneModel;

    private List<PhoneMessage> MessageBuffer;

    private void Awake()
    {
        MessageBuffer = new List<PhoneMessage>();

        UIView.DialogueBoxElement.AddMessage(new PhoneMessage
        {
            SenderType = SenderType.Operator,
            Message = Settings.LostIntroductionMessage
        });
        UIView.DialogueBoxElement.AddMessage(new PhoneMessage
        {
            SenderType = SenderType.Operator,
            Message = "What is your emergency?"
        });
    }

    private void Update()
    {
        UpdateHUD();
        UpdateNetwork();
        UpdateMessages();
        UpdateDialogue();
        UpdateTextBox();
        UpdateFlashlight();
        UpdateBattery();
    }

    private void UpdateNetwork()
    {
        var hits = Physics.OverlapSphere(transform.position, 0.2f, LayerMask.GetMask("Network"), QueryTriggerInteraction.Collide);

        if (hits.Length == 0)
        {
            PhoneModel.SetNetworkLevel(0);
            return;
        }

        SphereCollider zone = hits[0].GetComponent<SphereCollider>();
        float radius = zone.radius * zone.transform.lossyScale.x;
        float distance = Vector3.Distance(transform.position, zone.transform.TransformPoint(zone.center));
        float t = distance / radius;
        int networkValue = Mathf.RoundToInt((1f - t) * 4f);
        PhoneModel.SetNetworkLevel(Mathf.Clamp(networkValue, 0, 4));
    }

    private void UpdateHUD()
    {
        UIView.BatteryElement.SetLevel(PhoneModel.GetBatteryLevel());
        UIView.NetworkElement.SetLevel(PhoneModel.GetNetworkLevel());
        UIView.NewMessageImage.enabled = PhoneModel.HasUnreadMessages();

        if (PhoneModel.HasUnreadMessages() && UIView.DialogueBoxElement.IsAtBottom())
        {
            PhoneModel.SetNewMessagesRead();
        }
    }

    private void UpdateMessages()
    {
        if (PhoneModel.GetNetworkLevel() <= 0)
        {
            return;
        }

        float drain = 0;

        if (NetModel.ReceivedMessages(MessageBuffer))
        {
            drain += Settings.MessageRecievedBatteryDrain;

            foreach (PhoneMessage message in MessageBuffer)
            {
                PhoneModel.AddToMessagesBuffer(message);
                drain += GetBatteryDrain(message.Message);
            }
        }

        PhoneModel.SetBatteryLevel(PhoneModel.GetBatteryLevel() - drain);
    }

    private void UpdateDialogue()
    {
        if (PhoneModel.IsTypingEnabled())
        {
            return;
        }

        var keyboard = Keyboard.current;

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

        if (PhoneModel.GetNetworkLevel() > 0)
        {
            MessageBuffer.Clear();

            if (PhoneModel.DrainMessagesBuffer(MessageBuffer))
            {
                foreach (PhoneMessage message in MessageBuffer)
                {
                    UIView.DialogueBoxElement.AddMessage(message);
                }
            }
        }
    }

    private void UpdateTextBox()
    {
        if (PhoneModel.IsTypingEnabled() == false)
        {
            UIView.MessageBoxElement.SetTypingDisabled();
            return;
        }

        UIView.MessageBoxElement.SetTypingEnabled();

        var keyboard = Keyboard.current;

        UpdateMessageLabels();

        var canSendMessage = PhoneModel.GetNetworkLevel() > 0 &&
            PhoneModel.IsWaitingMessage(Settings.MessageSendInterval) == false;

        var sendKeyPressed = HadControl(Settings.SendMessageKey);

        if (canSendMessage)
        {
            if (sendKeyPressed)
            {
                string text = UIView.MessageBoxElement.GetText();

                float drain = Settings.MessageSendBatteryDrain + GetBatteryDrain(text);
                PhoneModel.SetBatteryLevel(PhoneModel.GetBatteryLevel() - drain);
                
                var message = new PhoneMessage { SenderType = SenderType.Lost, Message = text };
                
                NetModel.SendMessage(message);
                
                PhoneModel.RecordMessageSent();
                
                UIView.MessageBoxElement.Clear();
                UIView.MessageBoxElement.Activate();

                UIView.DialogueBoxElement.AddMessage(message);
                UIView.DialogueBoxElement.ScrollToBottom();
                return;
            }
        }

        if (HadControl(Settings.QuitWritingKey))
        {
            PhoneModel.ToggleTypingEnabled();
            UpdateMessageLabels();
        }

        UIView.MessageBoxElement.Activate();
    }

    private float GetBatteryDrain(string text)
    {
        return text.Length * Settings.MessagePerSymbolDrain;
    }

    private void UpdateMessageLabels()
    {
        if (PhoneModel.IsTypingEnabled() == false)
        {
            UIView.HasNetworkGroup.SetActive(false);
            UIView.NoNetworkGroup.SetActive(false);
            UIView.InstructionsGroup.SetActive(true);
            UIView.WaitingGroup.SetActive(false);
            return;
        }

        if (PhoneModel.GetNetworkLevel() <= 0)
        {
            UIView.HasNetworkGroup.SetActive(false);
            UIView.NoNetworkGroup.SetActive(true);
            UIView.WaitingGroup.SetActive(false);
            UIView.InstructionsGroup.SetActive(false);
            return;
        }

        if (PhoneModel.IsWaitingMessage(Settings.MessageSendInterval))
        {
            UIView.HasNetworkGroup.SetActive(false);
            UIView.NoNetworkGroup.SetActive(false);
            UIView.WaitingGroup.SetActive(true);
            UIView.InstructionsGroup.SetActive(false);
            return;
        }

        UIView.HasNetworkGroup.SetActive(true);
        UIView.NoNetworkGroup.SetActive(false);
        UIView.WaitingGroup.SetActive(false);
        UIView.InstructionsGroup.SetActive(false);
    }

    private void UpdateFlashlight()
    {
        if (PhoneModel.IsTypingEnabled())
        {
            return;
        }

        if (HadControl(Settings.FlashlightKey))
        {
            PhoneModel.ToggleFlashLight();
        }

        SceneView.FlashLight.SetActive(PhoneModel.IsFlashOn);

        if (PhoneModel.IsFlashOn)
        {
            if (PhoneModel.IsControlEnabled())
            {
                PhoneModel.SetBatteryLevel(PhoneModel.GetBatteryLevel() - Settings.FlashLightBatteryDrain * Time.deltaTime);
            }
        }

    }

    private void UpdateBattery()
    {
        if (PhoneModel.IsControlEnabled())
        {
            PhoneModel.SetBatteryLevel(PhoneModel.GetBatteryLevel() - Settings.PassiveBatteryDrain * Time.deltaTime);
        }

        if (PhoneModel.GetBatteryLevel() > 0)
        {
            return;
        }

        UIView.BatteryElement.SetLevel(0);

        PhoneModel.TurnOff();

        if (PhoneModel.IsFlashOn)
        {
            PhoneModel.ToggleFlashLight();
        }

        SceneView.FlashLight.SetActive(false);
        SceneView.ScreenLight.SetActive(false);
        UIView.MessageBoxElement.SetTypingDisabled();
        PhoneModel.DisableControl();
        //UIView.Content.SetActive(false);
        enabled = false;
    }


    private bool HadControl(Key key)
    {
        if (PhoneModel.IsControlEnabled())
        {
            var keyboard = Keyboard.current;
            return keyboard[key].wasPressedThisFrame;
        }

        return false;
    }
}
