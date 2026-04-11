using System.Collections;
using UnityEngine;

public class UnstableSpot : MonoBehaviour
{
    public FallingBlock FallingBlock;
    public GameObject Particles;
    public float StartEarthquakeTime = 2f;
    public float FallBlockTime = 5f;
    public float ShakeRange = 3f;

    private Collider TriggerCollider;
    private Transform PlayerTransform;
    private bool Activated;

    private void Awake()
    {
        TriggerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (Activated)
        {
            return;
        }

        Activated = true;
        PlayerTransform = other.transform.root;
        TriggerCollider.enabled = false;

        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        yield return new WaitForSeconds(StartEarthquakeTime);

        Particles.SetActive(true);

        float rampDuration = FallBlockTime - StartEarthquakeTime;
        float elapsed = 0f;

        while (elapsed < rampDuration)
        {
            elapsed += Time.deltaTime;
            float distanceFactor = GetDistanceFactor();
            CameraShake.Instance.Power = Mathf.Lerp(0f, distanceFactor, elapsed / rampDuration);
            yield return null;
        }
        Particles.SetActive(false);

        FallingBlock.StartFall();
    }

    private float GetDistanceFactor()
    {
        float distance = Vector3.Distance(PlayerTransform.position, transform.position);
        return distance <= ShakeRange ? 1f : 0.1f;
    }
}
