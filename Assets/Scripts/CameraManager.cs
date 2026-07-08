using TMPro;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //main camera
    public Camera mainCamera;

    //grid of terminals
    [SerializeField]
    GridManger grid;

    //variables for zoom and pan
    private float targetSize, cameraSpeed, panSpeed, minSize, maxSize;
    private Vector3 targetPosition;

    void Awake()
    {
        //defines the camera and aspect ratio 
        mainCamera = Camera.main;
        float aspectRatio = (float)Screen.width / Screen.height;

        //sets the zoom variables
        minSize = 3 / aspectRatio;
        maxSize = grid.gridSize / aspectRatio;
        targetSize = maxSize;
        cameraSpeed = 15f;
        panSpeed = 25f;

        //sets the camera size 
        mainCamera.orthographicSize = targetSize;
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
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, cameraSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Called when the touch manager detects pan to apply camera pan.
    /// </summary>
    public void ApplyPan(Vector2 delta)
    {
        if (mainCamera.orthographicSize == maxSize) return;

        //bounds
        float vertPanRange = maxSize - mainCamera.orthographicSize;
        float horzPanRange = vertPanRange * mainCamera.aspect;

        //sets target size according to the fingers' distance
        Vector3 worldDelta = mainCamera.ScreenToWorldPoint(Vector3.zero) - mainCamera.ScreenToWorldPoint((Vector3)delta);
        targetPosition = mainCamera.transform.position;
        targetPosition += worldDelta;

        //clamps range with bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, -horzPanRange, horzPanRange);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -vertPanRange, vertPanRange);

        //applies delta to camera with smoothening
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, panSpeed * Time.deltaTime);
    }
}
