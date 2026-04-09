using UnityEngine;
using UnityEngine.InputSystem;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [Range(0f, 1f)]
    public float Power = 0.5f;
    public float MaxAngle     = 3f;
    public float MaxOffset    = 0.1f;
    public float TraumaDecay  = 1.5f;

    private float Trauma = 0f;
    private Vector3 OriginPosition;
    private Quaternion OriginRotation;

    private void Awake()
    {
        OriginPosition = transform.localPosition;
        OriginRotation = transform.localRotation;

        Trauma = TraumaDecay;
    }

    private void Update()
    {
        float shake = Mathf.Pow(Power, 2f);

        float offsetX = MaxOffset * shake * (Mathf.PerlinNoise(Time.time * 8f, 0f) * 2f - 1f);
        float offsetY = MaxOffset * shake * (Mathf.PerlinNoise(0f, Time.time * 8f) * 2f - 1f);

        float angleZ = MaxAngle * shake * (Mathf.PerlinNoise(Time.time * 8f, 99f) * 2f - 1f);
        float angleX = MaxAngle * shake * (Mathf.PerlinNoise(Time.time * 8f, 42f) * 2f - 1f);

        transform.localPosition = OriginPosition + new Vector3(offsetX, offsetY, 0f);
        transform.localRotation = OriginRotation * Quaternion.Euler(angleX, 0f, angleZ);

        //Trauma = Mathf.Max(0f, Trauma - TraumaDecay * Time.deltaTime);
    }

    public void Shake(float amount = 1f)
    {
        Trauma = Mathf.Clamp01(Trauma + amount);
    }
}
