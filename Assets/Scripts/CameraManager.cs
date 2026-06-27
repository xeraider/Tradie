using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //main camera
    public Camera mainCamera;

    //grid of waternodes
    [SerializeField]
    GridManger grid;

    //variables for zoom
    private float targetZoom, zoomSpeed, minZoom, maxZoom, smooth;

    void Awake()
    {
        //Defines the camera and aspect ratio 
        mainCamera = Camera.main;
        float aspectRatio = (float)Screen.width / Screen.height;

        //Defines the minimum and maximum zoom according to the grid
        maxZoom = (grid.gridSize) / (aspectRatio);
        minZoom = 3 / aspectRatio;

        //Sets both the camera size and target zoom 
        mainCamera.orthographicSize = maxZoom;
        targetZoom = maxZoom;

        //Sets zoom speed or smoothening factor
        zoomSpeed = 0.03f;
        smooth = 0.4f;
    }

    void LateUpdate()
    {
        //lerp fn used to smoothly transition camera
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, smooth * Time.deltaTime);
    }

    /// <summary>
    /// Called when the screen is pinched and applies delta from the two finger positions to the camera.
    /// </summary>
    public void ApplyZoom(float delta)
    {
        //target zoom is set according to the delta
        targetZoom -= delta * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }
}
