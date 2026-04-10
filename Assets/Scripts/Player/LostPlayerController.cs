using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LostPlayerController : MonoBehaviour
{
    public LostPhoneModel PhoneModel;
    public LostPlayerModel PlayerModel;
    public LostKeyboardController MoveController;

    private Rigidbody Body;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateLife();
    }

    private void UpdateMovement()
    {
        if (PhoneModel.IsTypingEnabled())
        {
            PlayerModel.DisableMovement();
            MoveController.IsMovingEnabled = false;
            return;
        }

        PlayerModel.EnableMovement();
        MoveController.IsMovingEnabled = true;
    }

    private void UpdateLife()
    {
        if (PlayerModel.IsAlive() == false)
        {
            PhoneModel.DisableControl();
            MoveController.enabled = false;
            Body.freezeRotation = false;
            enabled = false;
        }
    }
}
