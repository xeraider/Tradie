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
    private int gridSize = 4;

    public WaterNode[,] grid;

    // Screen, container and node spacing
    private float spacingX, spacingY;

    private void Start()
    {
        //Defines the initial camera size according to the grid
        Camera.main.orthographicSize = gridSize + 2;
        

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
                //spawns node 70 % of the time
                if (Random.Range(0, 1f) > 0.3f)
                {
                    //position of the water node
                    Vector2 pos = new Vector2((x - (gridSize - 1) / 2f) * spacingX, (y - (gridSize - 1) / 2f) * spacingY);

                    //creates the water node
                    GameObject waterNodeObject = Instantiate(waterNodePrefab, pos, Quaternion.identity);
                    WaterNode waterNode = waterNodeObject.GetComponent<WaterNode>();
                    waterNode.SetUp(x, y);

                    //Add water node to grid
                    grid[x, y] = waterNode;
                }
                else
                {
                    //add empty node
                    grid[x, y] = null;
                }
            }
        }
    }
}
