using UnityEngine;

/// <summary>
/// Manages the grid of water nodes.
/// </summary>
public class GridManger : MonoBehaviour
{
    [SerializeField] 
    private GameObject waterNodePrefab;

    [SerializeField]
    private int gridSize = 4;

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
        waterNodePrefab.transform.localScale = new Vector3(Mathf.Min(spacingX, spacingY) / 2f, Mathf.Min(spacingX, spacingY) / 2f, 0f);

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
                Vector2 pos =  new Vector2((x + 1) * spacingX - containerWidth / 2, (y + 1) * spacingY - containerHeight / 2);

                GameObject waterNodeObject  = Instantiate(waterNodePrefab, pos, Quaternion.identity);
                WaterNode waterNode = waterNodeObject.GetComponent<WaterNode>();
                waterNode.gridX = x + 1;
                waterNode.gridY = y + 1;
                waterNode.name = "waterNode(" + x + ", " + y + ")";
            }
        }
    }
}
