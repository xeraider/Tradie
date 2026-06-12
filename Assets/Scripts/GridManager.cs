using UnityEngine;

/// <summary>
/// Manages the grid of water nodes.
/// </summary>
public class GridManger : MonoBehaviour
{
    private int gridSize = 4;

    [SerializeField] 
    private GameObject waterNodePrefab;


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

        //Sets the sizes of the container and the water nodes
        waterNodePrefab.transform.localScale = new Vector3(Mathf.Min(containerWidth, containerHeight) / (gridSize * 2), Mathf.Min(containerWidth, containerHeight) / (gridSize * 2), 0f);

        //Calls methond to draw nodes
        DrawNodes();
    }

    /// <summary>
    /// Called to Draw the grid of water nodes on the screen.
    /// </summary>
    private void DrawNodes()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 pos =  new Vector3((x + 1) * spacingX - containerWidth / 2, (y + 1) * spacingY - containerHeight / 2, 0f);

                GameObject waterNode  = Instantiate(waterNodePrefab, pos, Quaternion.identity);
                waterNode.name = "waterNode(" + x + ", " + y + ")";
            }
        }
    }
}
