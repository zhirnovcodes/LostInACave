using UnityEngine;

public class OperatorGameController : MonoBehaviour
{
    public NetModelBase NetModel;
    public OperatorPhoneController PhoneController;
    public OperatorPhoneModel PhoneModel;
    public OperatorOverlayUIView OverlayUIView;
    public InstructionsBookController BookController;
    public OperatorHUDView HUDView;

    private int Stage;

    private void Awake()
    {
        NetModel.SendConnected();
        OverlayUIView.Content.SetActive(true);
        OverlayUIView.ShowWaiting();
        PhoneModel.DisableControl();

        HUDView.Book.onClick.AddListener(() => BookController.Open());
    }

    private void Update()
    {
        if (NetModel.HasOpponentConnected())
        {
            OverlayUIView.Content.SetActive(false);
            PhoneModel.EnableControl();
        }
        else
        {
            OverlayUIView.Content.SetActive(true);
            OverlayUIView.ShowWaiting();
            PhoneModel.DisableControl();
        }

        if (NetModel.IsDeadReceived())
        {
            PhoneController.enabled = false;
            this.enabled = false;
            OverlayUIView.Content.SetActive(true);
            OverlayUIView.ShowGameOver();
            return;
        }

        if (NetModel.IsWonReceived())
        {
            PhoneController.enabled = false;
            this.enabled = false;
            OverlayUIView.Content.SetActive(true);
            OverlayUIView.ShowVictory();
        }
    }
}
