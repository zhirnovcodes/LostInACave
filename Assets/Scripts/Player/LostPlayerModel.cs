using UnityEngine;

public class LostPlayerModel : MonoBehaviour
{
    [SerializeField] private LostPlayerData Data;

    public void Kill()
    {
        Data.IsAlive = false;
    }

    public bool IsAlive()
    {
        return Data.IsAlive;
    }

    public void EnableMovement()
    {
        Data.IsMovingEnabled = true;
    }

    public void DisableMovement()
    {
        Data.IsMovingEnabled = false;
    }
}
