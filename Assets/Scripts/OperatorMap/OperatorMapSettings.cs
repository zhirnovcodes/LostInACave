using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "OperatorMapSettings", menuName = "OperatorMap/OperatorMapSettings")]
public class OperatorMapSettings : ScriptableObject
{
    [Header("Camera")]
    public float CameraMoveSpeed;

    [Header("Keys")]
    public Key ZoomInKey;
    public Key ZoomOutKey;
}
