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

    //player
    private PlayerInput playerInput;

    //input actions for touch
    private InputAction touchPressAction, touchPosition1Action;

    //bool to check if the fingers are in a pinching motion
    private bool isPinching;

    //previous distance between the two fingers
    private float previousDistance;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        //get input actions from Input System
        touchPressAction = playerInput.actions.FindAction("TouchPress");
        touchPosition1Action = playerInput.actions.FindAction("TouchPosition1");

        //errors for debugging touch actions
        Debug.Assert(touchPressAction != null, "TouchPress action not found.");
        Debug.Assert(touchPosition1Action != null, "TouchPositionAction action not found.");
    }

    private void Update()
    {
        //need two active touches
        if (Touch.activeTouches.Count < 2)
        {
            isPinching = false;
            return;
        }

        //sets position of two fingers
        Vector2 touchPosition1 = Touch.activeTouches[0].screenPosition;
        Vector2 touchPosition2 = Touch.activeTouches[1].screenPosition;

        //distance between touch positions
        float currentDistance = Vector2.Distance(touchPosition1, touchPosition2);

        //if its not pinching return
        if (!isPinching)
        {
            isPinching = true;
            previousDistance = currentDistance;
            return;
        }

        //delta difference tracking change in distance between two fingers
        float delta = currentDistance - previousDistance;
        previousDistance = currentDistance;

        //apply delta to the camera zoom
        cameraManager.ApplyZoom(delta);
    }

    private void OnEnable()
    {
        touchPressAction.started += Press;

    }

    private void OnDisable()
    {
        touchPressAction.started -= Press;
    }

    /// <summary>
    /// Called when the screen is tapped.
    /// </summary>
    private void Press(InputAction.CallbackContext context)
    {
        //gets the postion of the touch according to the world positions
        Vector2 touchPosition = cameraManager.mainCamera.ScreenToWorldPoint(
            touchPosition1Action.ReadValue<Vector2>());

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

}