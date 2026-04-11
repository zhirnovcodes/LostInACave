using UnityEngine;
using UnityEngine.InputSystem;

public class OperatorMapController : MonoBehaviour
{
    public OperatorMapSettings Settings;
    public OperatorMapModel Model;
    public OperatorMapSceneView SceneView;

    private void Update()
    {
        UpdateZoom();
        UpdatePosition();
    }

    private void UpdateZoom()
    {
        if (Model.IsZoomed())
        {
            if (IsControlled(Settings.ZoomOutKey))
            {
                Model.ToggleZoom();
                SceneView.MapCamera.transform.position = SceneView.CameraCentralPosition.position;
            }

            return;
        }

        if (IsControlled(Settings.ZoomInKey))
        {
            SceneView.MapCamera.transform.position = SceneView.ZoomedPosition;
            Model.ToggleZoom();
        }
    }

    private void UpdatePosition()
    {
        if (Model.IsZoomed() == false)
        {
            return;
        }

        if (Model.IsControlled() == false)
        {
            return;
        }

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        Vector3 cameraPosition = SceneView.MapCamera.transform.position;

        cameraPosition.x += mouseDelta.x * Settings.CameraMoveSpeed * Time.deltaTime;
        cameraPosition.z += mouseDelta.y * Settings.CameraMoveSpeed * Time.deltaTime;

        cameraPosition.x = Mathf.Clamp(
            cameraPosition.x,
            SceneView.ZoomedPosition.x - SceneView.ZoomedMapScale.x,
            SceneView.ZoomedPosition.x + SceneView.ZoomedMapScale.x);

        cameraPosition.z = Mathf.Clamp(
            cameraPosition.z,
            SceneView.ZoomedPosition.z - SceneView.ZoomedMapScale.y,
            SceneView.ZoomedPosition.z + SceneView.ZoomedMapScale.y);

        cameraPosition.y = SceneView.ZoomedPosition.y;

        SceneView.MapCamera.transform.position = cameraPosition;
    }

    private bool IsControlled(Key key)
    {
        if (Model.IsControlled())
        {
            return Keyboard.current[key].wasPressedThisFrame;
        }

        return false;
    }
}
