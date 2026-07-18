using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles touch input.
/// </summary>
public class TouchManager : MonoBehaviour
{
    //manages connections
    [SerializeField]
    private ConnectionManager connectionManager;

    //manages camera
    [SerializeField]
    private CameraManager cameraManager;

    //player input
    private PlayerInput playerInput;

    //input actions for touch
    private InputAction touchContactAction, touch2ContactAction, touchPositionAction, touch2PositionAction;

    private Vector2 touchStartPosition;

    //coroutine for zoom
    private Coroutine cameraCoroutine;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        //get input actions from Input System
        touchContactAction = playerInput.actions.FindAction("TouchContact");
        touch2ContactAction = playerInput.actions.FindAction("Touch2Contact");
        touchPositionAction = playerInput.actions.FindAction("TouchPosition");
        touch2PositionAction = playerInput.actions.FindAction("Touch2Position");
    }

    private void OnEnable()
    {
        touchContactAction.started += TouchStart;
        touchContactAction.canceled += TouchEnd;
    }

    private void OnDisable()
    {
        touchContactAction.started -= TouchStart;
        touchContactAction.canceled -= TouchEnd;
    }

    /// <summary>
    /// Called when the screen is tapped.
    /// </summary>
    private void Press()
    {
        //gets the postion of the touch according to the world positions
        Vector2 touchPosition = cameraManager.MainCamera.ScreenToWorldPoint(
            touchPositionAction.ReadValue<Vector2>());

        //finds the terminal that is tapped
        Collider2D target = Physics2D.OverlapPoint(touchPosition);

        //if nothing is selected
        if (target == null)
        {
            connectionManager.SelectTerminal(null);
            return;
        }

        //processes selection
        if (target.TryGetComponent(out Terminal terminal))
        {
            connectionManager.SelectTerminal(terminal);
            return;
        }
        else if (target.TryGetComponent(out Connection connection))
        {
            connectionManager.SelectConnection(connection);
            return;
        }
    }

    /// <summary>
    /// Called when the finger pinch starts.
    /// </summary>
    private void TouchStart(InputAction.CallbackContext context)
    {
        //records start touch position
        touchStartPosition = touchPositionAction.ReadValue<Vector2>();

        //start the zoom and pan
        cameraCoroutine = StartCoroutine(TouchDetection());
    }

    /// <summary>
    /// Called when the finger pinch ends.
    /// </summary>
    private void TouchEnd(InputAction.CallbackContext context)
    {
        //stop the zoom and pan
        StopCoroutine(cameraCoroutine);

        //if finger didnt move past the threshhold then activate press
        if (Vector2.Distance(touchStartPosition, touchPositionAction.ReadValue<Vector2>()) < 2f)
        {
            Press();
        }
    }

    /// <summary>
    /// Coroutine for the camera function.
    /// </summary>
    IEnumerator TouchDetection()
    {
        //previous variables
        Vector2 previousTouchPosition = touchPositionAction.ReadValue<Vector2>();
        float previousDistance = Vector2.Distance(touchPositionAction.ReadValue<Vector2>(), touch2PositionAction.ReadValue<Vector2>());

        while (touchContactAction.IsPressed()) {

            if (touch2ContactAction.IsPressed())
            {
                //gets distance between the two fingers 
                float currentDistance = Vector2.Distance(touchPositionAction.ReadValue<Vector2>(), touch2PositionAction.ReadValue<Vector2>());
                float delta = (previousDistance - currentDistance);

                //apply to camera
                cameraManager.ApplyZoom(delta * 0.02f);

                //set previous distance for loop
                previousDistance = currentDistance;
                previousTouchPosition = touchPositionAction.ReadValue<Vector2>();
            }
            else {
                //gets change of position of the first finger
                Vector2 currentTouchPosition = touchPositionAction.ReadValue<Vector2>();
                Vector2 delta = currentTouchPosition - previousTouchPosition;

                //apply to camera
                cameraManager.ApplyPan(delta);

                //set previous postition for loop
                previousTouchPosition = currentTouchPosition;
            }

            yield return null;
        }
    }
}