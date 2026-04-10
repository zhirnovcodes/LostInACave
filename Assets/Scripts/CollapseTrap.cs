using UnityEngine;

public class CollapseTrap : MonoBehaviour
{
    public FallingBlock Block;
    public Collider Collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Collider.enabled = false;
            Block.StartFall();
        }
    }
}
