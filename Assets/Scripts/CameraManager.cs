using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class CameraManager : MonoBehaviour
{
    //main camera
    public Camera mainCamera;

    //grid of waternodes
    [SerializeField]
    GridManger grid;

    //variables for zoom
    private float targetSize, cameraSpeed, minSize, maxSize;

    void Awake()
    {
        //defines the camera and aspect ratio 
        mainCamera = Camera.main;
        float aspectRatio = (float)Screen.width / Screen.height;

        //ets the zoom variables
        minSize = 3 / aspectRatio;
        maxSize = grid.gridSize / aspectRatio;
        targetSize = maxSize;
        cameraSpeed = 10f;

        //ets the camera size 
        mainCamera.orthographicSize = targetSize;
    }

    /// <summary>
    /// Called when the touch manager detects pinch to apply camera zoom.
    /// </summary>
    public void ApplyZoom(float delta)
    {
        //sets target size according to the fingers' distance
        targetSize = mainCamera.orthographicSize;
        targetSize = targetSize + delta;
        targetSize = Mathf.Clamp(targetSize, minSize, maxSize);

        //applies delta to camera with smoothening
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * cameraSpeed);
    }
}
