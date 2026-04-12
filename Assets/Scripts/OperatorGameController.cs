using UnityEngine;

public class OperatorGameController : MonoBehaviour
{
    public OperatorPhoneController PhoneController;
    public OperatorPhoneModel PhoneModel;
    public OperatorOverlayUIView OverlayUIView;
    public InstructionsBookController BookController;
    public OperatorHUDView HUDView;

    private int Stage;
    private bool IsInitialized;

    private void Awake()
    {
        NetModelBase.Instance.SendSceneStarted();
        OverlayUIView.Content.SetActive(true);
        OverlayUIView.ShowWaiting();
        PhoneModel.DisableControl();

        HUDView.Book.onClick.AddListener(() => BookController.Open());
    }

    private void Update()
    {
        if (NetModelBase.Instance.HasOpponentSceneStarted())
        {
            OverlayUIView.Content.SetActive(false);
            PhoneModel.EnableControl();
        }
        else
        {
            OverlayUIView.Content.SetActive(true);
            OverlayUIView.ShowWaiting();
            PhoneModel.DisableControl();
            return;
        }

        if (NetModelBase.Instance.IsDeadReceived())
        {
            PhoneController.enabled = false;
            this.enabled = false;
            OverlayUIView.Content.SetActive(true);
            OverlayUIView.ShowGameOver();
            return;
        }

        if (NetModelBase.Instance.IsWonReceived())
        {
            PhoneController.enabled = false;
            this.enabled = false;
            OverlayUIView.Content.SetActive(true);
            OverlayUIView.ShowVictory();
        }
    }
}
