using System.Collections.Generic;
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

    //Water node that is currently selected
    private GameObject selectedWaterNode;

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
        Vector2 touchPosition = mainCamera.ScreenToWorldPoint(
            touchPositionAction.ReadValue<Vector2>());

        // Finds the node that is tapped
        Collider2D target = Physics2D.OverlapPoint(touchPosition);
        GameObject targetWaterNode = null;

        if(target != null)
        {
            targetWaterNode = target.gameObject;
        }
         

        if (targetWaterNode == null)
        {
            selectedWaterNode = null;
            return;
        }

        if (selectedWaterNode == targetWaterNode)
        {
            return;
        }

        if (selectedWaterNode == null)
        {
            selectedWaterNode = targetWaterNode.gameObject;
            return;
        }

        if (selectedWaterNode != targetWaterNode)
        {
            CreatePipes(selectedWaterNode, targetWaterNode);
            Debug.Log("Drawn like from " + targetWaterNode.name + " to " + selectedWaterNode.name);
            selectedWaterNode = null;
        }

    }

    private void CreatePipes(GameObject waterNodeA, GameObject waterNodeB)
    {
        WaterNode nodeA = waterNodeA.GetComponent<WaterNode>();
        WaterNode nodeB = waterNodeB.GetComponent<WaterNode>();

        if (nodeA.pipeConnections.Contains(nodeB))
        {
            Debug.Log("Pipe already exists");
            return;
        }

        nodeA.pipeConnections.Add(nodeB);
        nodeB.pipeConnections.Add(nodeA);

        GameObject pipe = new GameObject("Pipe");

        LineRenderer lr = pipe.AddComponent<LineRenderer>();

        lr.positionCount = 2;

        lr.SetPosition(0, waterNodeA.transform.position);
        lr.SetPosition(1, waterNodeB.transform.position);

        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.red;
        lr.endColor = Color.blue;
    }
}