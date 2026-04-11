using UnityEngine;
using UnityEngine.InputSystem;

public class MapPinModel : MonoBehaviour
{
    public Material NormalMaterial;
    public Material HighlightedMaterial;

    private MeshRenderer MeshRenderer;
    private bool IsSnapping;
    private Camera MainCamera;
    private Plane SnapPlane;

    private void Awake()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        MainCamera = Camera.main;
        SnapPlane = new Plane(Vector3.up, transform.position);
    }

    private void Update()
    {
        if (IsSnapping)
        {
            SnapToCursor();
        }
    }

    private void OnMouseEnter()
    {
        MeshRenderer.material = HighlightedMaterial;
    }

    private void OnMouseExit()
    {
        if (IsSnapping)
        {
            return;
        }

        MeshRenderer.material = NormalMaterial;
    }

    private void OnMouseDown()
    {
        IsSnapping = !IsSnapping;

        if (IsSnapping == false)
        {
            MeshRenderer.material = NormalMaterial;
        }
    }

    private void SnapToCursor()
    {
        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (SnapPlane.Raycast(ray, out float distance))
        {
            transform.position = ray.GetPoint(distance);
        }
    }
}
