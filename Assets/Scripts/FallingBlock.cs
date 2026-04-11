using System.Collections;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    public Transform Block;
    public float TargetY;
    public float FallTime = 0.2f;
    public float EarthQuakeTime = 0.2f;

    private bool IsFalling;

    public void StartFall()
    {
        StartCoroutine(Fall());
        StartCoroutine(Earthquake());
    }

    private IEnumerator Fall()
    {
        IsFalling = true;

        Vector3 startPosition = Block.localPosition;
        Vector3 endPosition = new Vector3(startPosition.x, TargetY, startPosition.z);
        float elapsed = 0f;

        while (elapsed < FallTime)
        {
            elapsed += Time.deltaTime;
            Block.localPosition = Vector3.Lerp(startPosition, endPosition, elapsed / FallTime);
            yield return null;
        }

        Block.localPosition = endPosition;
        IsFalling = false;
    }

    private IEnumerator Earthquake()
    {
        float halfTime = EarthQuakeTime * 0.5f;
        float elapsed = 0f;

        while (elapsed < halfTime)
        {
            elapsed += Time.deltaTime;
            CameraShake.Instance.Power = Mathf.Lerp(0f, 1f, elapsed / halfTime);
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < halfTime)
        {
            elapsed += Time.deltaTime;
            CameraShake.Instance.Power = Mathf.Lerp(1f, 0f, elapsed / halfTime);
            yield return null;
        }

        CameraShake.Instance.Power = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsFalling == false)
        {
            return;
        }

        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        collision.gameObject.GetComponentInParent<LostPlayerModel>().Kill();
    }
}
