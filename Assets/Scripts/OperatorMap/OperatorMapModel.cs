using UnityEngine;

public class OperatorMapModel : MonoBehaviour
{
    [SerializeField] private OperatorMapData Data;

    public bool IsZoomed()
    {
        return Data.IsZoomed;
    }

    public void ToggleZoom()
    {
        Data.IsZoomed = !Data.IsZoomed;
    }

    public bool IsControlled()
    {
        return Data.IsControlled;
    }

    public void ToggleControl()
    {
        Data.IsControlled = !Data.IsControlled;
    }
}
