using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Manages the grid of water nodes.
/// </summary>
public class GridManger : MonoBehaviour
{
    //Water node prefab
    [SerializeField]
    private GameObject waterNodePrefab;

    // Size of the grid of Water Nodes
    [SerializeField]
    private int gridSize = 6;

    // Screen, container and node spacing
    private float screenWidth, screenHeight, containerWidth, containerHeight, spacingX, spacingY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //Defines the dimensions of the screen 
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth = screenHeight * Screen.width / Screen.height;

        //Defines the water node grid dimensions
        containerWidth = screenWidth;
        containerHeight = screenHeight * 0.8f;

        //Sets the spacing between the water node spawn points 
        spacingX = containerWidth / (gridSize + 1);
        spacingY = containerHeight / (gridSize + 1);

        //Calls methond to draw nodes
        InitialiseGrid();
    }

    /// <summary>
    /// Called to Draw the grid of water nodes on the screen.
    /// </summary>
    private void InitialiseGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //position of the water node
                Vector2 pos = new Vector2((x + 1) * spacingX - containerWidth / 2, (y + 1) * spacingY - containerHeight / 2);

                //creates the water node
                GameObject waterNodeObject = Instantiate(waterNodePrefab, pos, Quaternion.identity);
                WaterNode waterNode = waterNodeObject.GetComponent<WaterNode>();
                waterNode.transform.localScale = new Vector3(Mathf.Min(spacingX, spacingY) / 2f, Mathf.Min(spacingX, spacingY) / 2f, 0f);
                waterNode.SetUp("WaterNode(" + x + "," + y + ")", x, y);
            }
        }
    }
}
