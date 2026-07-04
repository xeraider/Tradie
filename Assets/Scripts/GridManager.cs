using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

/// <summary>
/// Manages the grid of water nodes.
/// </summary>
public class GridManger : MonoBehaviour
{
    //water node prefab
    [SerializeField]
    private GameObject waterNodePrefab;

    //size of the grid of Water Nodes
    [SerializeField]
    public int gridSize = 4;

    public WaterNode[,] grid;

    //screen, container and node spacing
    public float spacingX, spacingY;

    private void Start()
    {
        //Sets the spacing between the water node spawn points 
        spacingX = 2f;
        spacingY = 2f;

        //Calls method to create and populate grid
        PopulateGrid();
    }

    /// <summary>
    /// Called to Draw the grid of water nodes on the screen.
    /// </summary>
    private void PopulateGrid()
    {
        //Initialises grid
        grid = new WaterNode[gridSize,gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //position of the water node
                Vector2 pos = new Vector2((x - (gridSize - 1) / 2f) * spacingX, (y - (gridSize - 1) / 2f) * spacingY);

                //creates the water node
                GameObject waterNodeObject = Instantiate(waterNodePrefab, pos, Quaternion.identity);
                WaterNode waterNode = waterNodeObject.GetComponent<WaterNode>();
                waterNode.SetUp(x, y, Random.Range(0, 8));

                //Add water node to grid
                grid[x, y] = waterNode;
            }
        }
    }
}
