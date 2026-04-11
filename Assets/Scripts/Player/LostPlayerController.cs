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
        var isEnabled = PlayerModel.IsMovementEnabled() &&
            PhoneModel.IsTypingEnabled() == false;
        MoveController.IsMovingEnabled = isEnabled;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Exit"))
        {
            PlayerModel.SetWon();
        }
    }
}
