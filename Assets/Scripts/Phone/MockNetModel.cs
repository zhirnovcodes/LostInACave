using System.Collections.Generic;
using UnityEngine;

public class MockNetModel : NetModelBase
{
    // --- Phone / Game Scene ---

    public bool SceneStarted;
    public bool IsDead;
    public bool IsWon;

    // --- Lobby ---

    public bool IsConnected;
    public bool IsOpponentConnected;
    public bool IsSuccess;
    public bool ConnectedFirst;
    public bool SelectedCharacter;
    public CharacterType Character;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void QueueRandomMessage()
    {
        PendingMessages.Add(new PhoneMessage
        {
            SenderType = SenderType.Operator,
            Message = SampleTexts[Random.Range(0, SampleTexts.Length)],
        });
    }

    // --- Phone / Game Scene ---

    public override void SendSceneStarted()
    {
        Debug.Log("[MockNetModel] SendSceneStarted");
    }

    public override bool HasOpponentSceneStarted()
    {
        return SceneStarted;
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

    public override bool IsDeadReceived()
    {
        return IsDead;
    }

    public override void SendWon()
    {
        Debug.Log("[MockNetModel] SendWon");
    }

    public override bool IsWonReceived()
    {
        return IsWon;
    }

    // --- Lobby ---

    public override void Connect()
    {
        Debug.Log("[MockNetModel] Connect");
    }

    public override bool HasConnected(out ConnectionResultData data)
    {
        data = new ConnectionResultData
        {
            IsSuccess = IsSuccess,
            IsConnectedFirst = ConnectedFirst,
        };
        return IsConnected;
    }

    public override void SelectCharacter(CharacterType character)
    {
        Character = character;
        SelectedCharacter = true;
    }

    public override bool IsCharacterSelected()
    {
        return SelectedCharacter;
    }

    public override CharacterType HasSelected()
    {
        return Character;
    }

    public override bool HasOpponentConnected()
    {
        return IsOpponentConnected;
    }

    public override void SendConnected()
    {
        Debug.Log("[MockNetModel] SendConnected");
    }
}
