using UnityEngine;

/// <summary>
/// Handles Camera.
/// </summary>
public class CameraManager : MonoBehaviour
{
    //main camera
    public Camera MainCamera;

    //variables for zoom and pan
    private float aspectRatio, targetSize, zoomSpeed, panSpeed, minSize, maxSize;
    private Vector3 targetPosition;

    public void Awake()
    {
        //sets main camera
        MainCamera = Camera.main;

        //sets aspect ratio and minimum camera size
        aspectRatio = MainCamera.aspect;
        minSize = 3 / aspectRatio;

        //sets zoom and pan speeds
        zoomSpeed = 15f;
        panSpeed = 25f;

        SetCamera(minSize);
    }

    /// <summary>
    /// Called when the camera is resized.
    /// </summary>
    public void SetCamera(float GridSize)
    {
        //sets the max size and target size for camera
        maxSize = Mathf.Max(minSize, GridSize / aspectRatio);
        targetSize = maxSize;

        //sets the camera size
        MainCamera.orthographicSize = targetSize;

        //centres camera 
        MainCamera.transform.position = new Vector3(0, 0, MainCamera.transform.position.z);
    }

    /// <summary>
    /// Called when the touch manager detects pinch to apply camera zoom.
    /// </summary>
    public void ApplyZoom(float delta)
    {
        //sets target size according to the fingers' distance
        targetSize += delta;
        targetSize = Mathf.Clamp(targetSize, minSize, maxSize);

        //applies delta to camera with smoothening
        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);

        //centers the camera on max zoom threshhold
        if (MainCamera.orthographicSize >= (maxSize - 0.5)) MainCamera.transform.position = new Vector3(0,0, MainCamera.transform.position.z);
    }

    /// <summary>
    /// Called when the touch manager detects pan to apply camera pan.
    /// </summary>
    public void ApplyPan(Vector2 delta)
    {
        //disables pan on max zoom threshhold
        if (MainCamera.orthographicSize >= (maxSize - 0.5)) return;

        //bounds
        float vertPanRange = maxSize - MainCamera.orthographicSize;
        float horzPanRange = vertPanRange * MainCamera.aspect;

        //sets target size according to the fingers' distance
        Vector3 worldDelta = MainCamera.ScreenToWorldPoint(Vector3.zero) - MainCamera.ScreenToWorldPoint((Vector3)delta);
        targetPosition = MainCamera.transform.position;
        targetPosition += worldDelta;

        //clamps range with bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, -horzPanRange, horzPanRange);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -vertPanRange, vertPanRange);

        //applies delta to camera with smoothening
        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, targetPosition, panSpeed * Time.deltaTime);
    }
}
