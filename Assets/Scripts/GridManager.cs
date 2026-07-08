using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

/// <summary>
/// Manages the grid of terminals.
/// </summary>
public class GridManger : MonoBehaviour
{
    //terminal prefab
    [SerializeField]
    private GameObject terminalPrefab;

    //size of the grid of terminals
    [SerializeField]
    public int gridSize;

    public Terminal[,] grid;

    //screen, container and terminal spacing
    public float spacingX, spacingY;

    private void Start()
    {
        //Sets the spacing between the terminal spawn points 
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
        grid = new Terminal[gridSize,gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //position of the terminal
                Vector2 pos = new Vector2((x - (gridSize - 1) / 2f) * spacingX, (y - (gridSize - 1) / 2f) * spacingY);

                //creates the water node
                GameObject terminalObject = Instantiate(terminalPrefab, pos, Quaternion.identity);
                Terminal terminal = terminalObject.GetComponent<Terminal>();
                terminal.SetUp(x, y, Random.Range(0, 8));

                //Add terminal to grid
                grid[x, y] = terminal;
            }
        }
    }
}
