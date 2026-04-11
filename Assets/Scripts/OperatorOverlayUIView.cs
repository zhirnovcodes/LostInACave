using UnityEngine;

public class OperatorOverlayUIView : MonoBehaviour
{
    public GameObject Content;
    public GameObject Waiting;
    public GameObject GameOver;
    public GameObject Victory;

    public void ShowWaiting()
    {
        Waiting.SetActive(true);
        GameOver.SetActive(false);
        Victory.SetActive(false);
    }

    public void ShowGameOver()
    {
        Waiting.SetActive(false);
        GameOver.SetActive(true);
        Victory.SetActive(false);
    }

    public void ShowVictory()
    {
        Waiting.SetActive(false);
        GameOver.SetActive(false);
        Victory.SetActive(true);
    }
}
