using UnityEngine;
using UnityEngine.InputSystem;

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

        //If tapped on a node set the target node to the selected node
        if (target != null)
        {
            targetWaterNode = target.gameObject;
        }
         
        //If tapped on empty space then set the unselect the selected node
        if (targetWaterNode == null)
        {
            selectedWaterNode = null;
            return;
        }

        //If the selected node is the one being clicked on then do nothing
        if (selectedWaterNode == targetWaterNode)
        {
            return;
        }

        //if their is no selected node then set it to the tapped node
        if (selectedWaterNode == null)
        {
            selectedWaterNode = targetWaterNode.gameObject;
            return;
        }

        //Checks and makes sure the connections are not diagonal
        if (selectedWaterNode.transform.position.x != targetWaterNode.transform.position.x && selectedWaterNode.transform.position.y != targetWaterNode.transform.position.y)
        {
            Debug.Log("Pipes cannot connect diagonally");
            selectedWaterNode = null;
            return;
        }

        //Creates the connection and resets the selected node.
        CreatePipes(selectedWaterNode, targetWaterNode);
        Debug.Log("Connected " + targetWaterNode.name + " to " + selectedWaterNode.name);
        selectedWaterNode = null;

    }

    /// <summary>
    /// Creates pipe connection
    /// </summary>
    private void CreatePipes(GameObject waterNodeA, GameObject waterNodeB)
    {

        WaterNode nodeA = waterNodeA.GetComponent<WaterNode>();
        WaterNode nodeB = waterNodeB.GetComponent<WaterNode>();

        //Checks if the connection already exists
        if (nodeA.pipeConnections.Contains(nodeB))
        {
            Debug.Log("Pipe already exists");
            return;
        }

        //adds connection to each pipe
        nodeA.pipeConnections.Add(nodeB);
        nodeB.pipeConnections.Add(nodeA);

        //Create game object for pipe
        GameObject pipe = new GameObject("Pipe");

        //Line renderer draws the line which 
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