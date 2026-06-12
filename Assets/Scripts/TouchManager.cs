using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

/// <summary>
/// Handles touch input.
/// </summary>
public class TouchManager : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerInput playerInput;

    //Input actions for touch
    private InputAction touchPressAction;
    private InputAction touchPositionAction;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();

        // Get input actions from Input System
        touchPressAction = playerInput.actions.FindAction("TouchPress");
        touchPositionAction = playerInput.actions.FindAction("TouchPosition");

        //Errors for debugging touch actions
        Debug.Assert(touchPressAction != null, "TouchPress action not found.");
        Debug.Assert(touchPositionAction != null, "TouchPosition action not found.");
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
    }

    /// <summary>
    /// Called when the screen is tapped.
    /// </summary>
    private void TouchPressed(InputAction.CallbackContext context)
    {
        // Convert touch position to world position
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(
            touchPositionAction.ReadValue<Vector2>());
    }
}