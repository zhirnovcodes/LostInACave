using UnityEngine;

public class InstructionsBookController : MonoBehaviour
{
    public InstructionsBookUIView UIView;
    public PhoneSettings Settings;

    public int CurrentIndex;

    private void Awake()
    {
        for (int i = 0; i < UIView.Buttons.Length; i++)
        {
            int targetIndex = i;
            UIView.Buttons[i].onClick.AddListener(() => NavigateToPage(targetIndex));
        }

        UIView.CloseButton.onClick.AddListener(Close);

        UIView.Page1Text.text = UIView.Page1Text.text
            .Replace("<passive_drain>", Settings.PassiveBatteryDrain.ToString())
            .Replace("<flash_drain>", Settings.FlashLightBatteryDrain.ToString())
            .Replace("<send_message>", Settings.MessageSendBatteryDrain.ToString())
            .Replace("<receive_message>", Settings.MessageRecievedBatteryDrain.ToString())
            .Replace("<per_symbol>", Settings.MessagePerSymbolDrain.ToString());
    }

    public void Open()
    {
        UIView.Content.SetActive(true);
    }

    private void NavigateToPage(int targetIndex)
    {
        UIView.Pages[CurrentIndex].SetActive(false);
        CurrentIndex = targetIndex;
        UIView.Pages[CurrentIndex].SetActive(true);
        UIView.CurrentIndexText.text = (CurrentIndex+1).ToString();
    }

    private void Close()
    {
        UIView.Content.SetActive(false);
    }
}
