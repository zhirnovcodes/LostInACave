using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    public LobbyUIView UIView;
    public GameSceneManager SceneManager;

    private bool HasConnectedSent;

    private void Awake()
    {
        Application.runInBackground = true;

        UIView.WaitingLabel.SetActive(false);
        UIView.SelectButtonsGroup.SetActive(false);
        UIView.ErrorLabel.SetActive(false);
        UIView.ConnectGroup.SetActive(true);

        UIView.ConnectButton.onClick.AddListener(OnConnectPressed);
        UIView.PlayLostButton.onClick.AddListener(OnPlayLostPressed);
        UIView.PlayOperatorButton.onClick.AddListener(OnPlayOperatorPressed);

        Application.logMessageReceived += OnLogMessage;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= OnLogMessage;
    }

    private void OnLogMessage(string message, string stackTrace, LogType type)
    {
        UIView.Logs.text += $"[{type}] {message}\n";
    }

    private void Update()
    {
        var isConnected = NetModelBase.Instance.HasConnected(out var connectionResult);

        if (isConnected == false)
        {
            return;
        }

        if (connectionResult.IsSuccess == false)
        {
            UIView.ErrorLabel.SetActive(true);
            UIView.WaitingLabel.SetActive(false);
            UIView.SelectButtonsGroup.SetActive(false);
            return;
        }

        if (connectionResult.IsConnectedFirst)
        {
            if (NetModelBase.Instance.HasOpponentConnected())
            {
                UIView.WaitingLabel.SetActive(false);
                UIView.SelectButtonsGroup.SetActive(true);
            }
            else
            {
                UIView.WaitingLabel.SetActive(true);
                UIView.SelectButtonsGroup.SetActive(false);
            }
        }
        else
        {
            UIView.WaitingLabel.SetActive(true);
            UIView.SelectButtonsGroup.SetActive(false);

            if (HasConnectedSent == false)
            {
                NetModelBase.Instance.SendConnected();
                HasConnectedSent = true;
            }

            if (NetModelBase.Instance.IsCharacterSelected())
            {
                var otherCharacter = NetModelBase.Instance.HasSelected() == CharacterType.Lost
                    ? CharacterType.Operator
                    : CharacterType.Lost;

                SceneManager.OpenScene(otherCharacter);
            }
        }
    }

    private void OnConnectPressed()
    {
        UIView.WaitingLabel.SetActive(true);
        UIView.ConnectGroup.SetActive(false);
        NetModelBase.Instance.Connect();
    }

    private void OnPlayLostPressed()
    {
        NetModelBase.Instance.SelectCharacter(CharacterType.Lost);
        SceneManager.OpenScene(CharacterType.Lost);
    }

    private void OnPlayOperatorPressed()
    {
        NetModelBase.Instance.SelectCharacter(CharacterType.Operator);
        SceneManager.OpenScene(CharacterType.Operator);
    }
}
