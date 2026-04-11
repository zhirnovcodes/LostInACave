using UnityEngine;

public class InstructionsBookController : MonoBehaviour
{
    public InstructionsBookUIView UIView;

    public int CurrentIndex;

    private void Awake()
    {
        for (int i = 0; i < UIView.Buttons.Length; i++)
        {
            int targetIndex = i;
            UIView.Buttons[i].onClick.AddListener(() => NavigateToPage(targetIndex));
        }

        UIView.CloseButton.onClick.AddListener(Close);
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
        UIView.CurrentIndexText.text = CurrentIndex.ToString();
    }

    private void Close()
    {
        UIView.Content.SetActive(false);
    }
}
