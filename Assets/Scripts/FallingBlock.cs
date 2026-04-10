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
