using UnityEngine;

public class LostGameController : MonoBehaviour
{
    public NetModelBase NetModel;
    public OperatorOverlayUIView OverlayUIView;
    public LostPlayerModel PlayerModel;
    public LostPhoneModel PhoneModel;
    public CountDownUIView CountDownView;

    private bool CountdownStarted;

    private void Awake()
    {
        NetModel.SendSceneStarted();
        OverlayUIView.Content.SetActive(true);
        OverlayUIView.ShowWaiting();
        PhoneModel.DisableControl();
        PlayerModel.DisableMovement();
    }

    private void Update()
    {
        if (NetModel.HasOpponentSceneStarted())
        {
            OverlayUIView.Content.SetActive(false);
            PhoneModel.EnableControl();
            PlayerModel.EnableMovement();
        }
        else
        {
            OverlayUIView.Content.SetActive(true);
            OverlayUIView.ShowWaiting();
            PhoneModel.DisableControl();
            PlayerModel.DisableMovement();
            return;
        }

        if (PlayerModel.IsAlive() == false)
        {
            NetModel.SendDead();
            OverlayUIView.Content.SetActive(true);
            OverlayUIView.ShowGameOver();
            PhoneModel.DisableControl();
            CountDownView.Content.SetActive(false);
            CountDownView.StopCountdown();
            this.enabled = false;
            return;
        }

        if (PlayerModel.HasWon())
        {
            NetModel.SendWon();
            OverlayUIView.Content.SetActive(true);
            OverlayUIView.ShowVictory();
            PhoneModel.DisableControl();
            CountDownView.StopCountdown();
            CountDownView.Content.SetActive(false);
            PlayerModel.DisableMovement();
            this.enabled = false;
            return;
        }

        if (PhoneModel.IsPhoneOn() == false)
        {
            if (CountdownStarted == false)
            {
                CountDownView.Content.SetActive(true);
                CountDownView.StartCountdown();
                CountdownStarted = true;
            }
        }

        if (CountdownStarted && CountDownView.HasFinished)
        {
            PlayerModel.Kill();
        }
    }
}
