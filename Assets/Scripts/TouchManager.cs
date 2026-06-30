using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

/// <summary>
/// Handles touch input.
/// </summary>
public class TouchManager : MonoBehaviour
{
    [SerializeField]
    private ConnectionManager connections;

    [SerializeField]
    private CameraManager cameraManager;

    private PlayerInput playerInput;

    //input actions for touch
    private InputAction touchPressAction, touchPositionAction, touch2PositionAction, touch2ContactAction;

    //coroutine for zoom
    private Coroutine zoomCoroutine;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        //get input actions from Input System
        touchPressAction = playerInput.actions.FindAction("TouchPress");
        touchPositionAction = playerInput.actions.FindAction("TouchPosition");
        touch2PositionAction = playerInput.actions.FindAction("Touch2Position");
        touch2ContactAction = playerInput.actions.FindAction("touch2Contact");
    }

    private void OnEnable()
    {
        touchPressAction.started += Press;
        touch2ContactAction.started += ZoomStart;
        touch2ContactAction.canceled += ZoomEnd;
    }

    private void OnDisable()
    {
        touchPressAction.started -= Press;
        touch2ContactAction.started -= ZoomStart;
        touch2ContactAction.canceled -= ZoomEnd;
    }

    /// <summary>
    /// Called when the screen is tapped.
    /// </summary>
    private void Press(InputAction.CallbackContext context)
    {
        //gets the postion of the touch according to the world positions
        Vector2 touchPosition = cameraManager.mainCamera.ScreenToWorldPoint(
            touchPositionAction.ReadValue<Vector2>());

        //finds the node that is tapped
        Collider2D target = Physics2D.OverlapPoint(touchPosition);

        //If nothing is selected
        if (target == null)
        {
            connections.SelectWaterNode(null);
            return;
        }

        //processes selection
        if (target.TryGetComponent(out WaterNode waterNode))
        {
            connections.SelectWaterNode(waterNode);
            return;
        }
        else if (target.TryGetComponent(out Connection connection))
        {
            connections.TapConnection(connection);
            return;
        }
    }

    /// <summary>
    /// Called when the finger pinch starts.
    /// </summary>
    private void ZoomStart(InputAction.CallbackContext context)
    {
        //start the zoom
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    /// <summary>
    /// Called when the finger pinch ends.
    /// </summary>
    private void ZoomEnd(InputAction.CallbackContext context)
    {
        //stop the zoom
        StopCoroutine(zoomCoroutine);
    }

    /// <summary>
    /// Coroutine for the zoom function.
    /// </summary>
    IEnumerator ZoomDetection()
    {
        float previousDistance = Vector2.Distance(touchPositionAction.ReadValue<Vector2>(), touch2PositionAction.ReadValue<Vector2>()), distance = 0f, delta = 0f;

        while (true) {
            //gets distance between the two fingers and sends it to the camera manager
            distance = Vector2.Distance(touchPositionAction.ReadValue<Vector2>(), touch2PositionAction.ReadValue<Vector2>());
            delta = (previousDistance - distance);
            cameraManager.ApplyZoom(delta * 0.02f);
            previousDistance = distance;
            yield return null;
        }
    }
}