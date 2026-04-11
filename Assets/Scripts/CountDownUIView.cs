using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CountText;
    public GameObject Content;

    private bool Finished;
    private Coroutine Routine;

    public bool HasFinished => Finished;

    public void StartCountdown()
    {
        Finished = false;
        Routine = StartCoroutine(RunCountdown());
    }

    public void StopCountdown()
    {
        if (Routine == null)
        {
            return;
        }

        StopCoroutine(Routine);
    }

    private IEnumerator RunCountdown()
    {
        for (int count = 10; count >= 0; count--)
        {
            CountText.text = count.ToString();
            yield return new WaitForSeconds(1f);
        }

        Finished = true;
    }
}
