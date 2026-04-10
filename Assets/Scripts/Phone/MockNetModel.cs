using System.Collections.Generic;
using UnityEngine;

public class MockNetModel : NetModelBase
{
    private static readonly string[] SampleTexts =
    {
        "Hello, is anyone there?",
        "Can you hear me?",
        "We are sending help!",
        "Stay where you are.",
        "What is your location?",
        "Are you injured?",
        "Keep your phone on.",
        "Rescue team is nearby.",
    };

    private List<PhoneMessage> PendingMessages = new List<PhoneMessage>();

    public void QueueRandomMessage()
    {
        PhoneMessage message = new PhoneMessage
        {
            SenderType = SenderType.Operator,
            Message = SampleTexts[Random.Range(0, SampleTexts.Length)],
        };
        PendingMessages.Add(message);
    }

    public override void SendConnected()
    {
        Debug.Log("[MockNetModel] SendConnected");
    }

    public override bool HasOpponentConnected()
    {
        return true;
    }

    public override void SendMessage(PhoneMessage message)
    {
        Debug.Log($"[MockNetModel] SendMessage: {message}");
    }

    public override bool ReceivedMessages(List<PhoneMessage> messages)
    {
        if (PendingMessages.Count == 0)
        {
            return false;
        }

        foreach (PhoneMessage message in PendingMessages)
        {
            messages.Add(message);
        }

        PendingMessages.Clear();
        return true;
    }

    public override void SendDead()
    {
        Debug.Log("[MockNetModel] SendDead");
    }
}
