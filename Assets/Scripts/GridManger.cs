using UnityEngine;

public class GridManger : MonoBehaviour
{
    private int gridSize = 6;

    [SerializeField] 
    private GameObject waterNodePrefab;

    private float screenWidth, screenHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Sets the dimensions of the screen 
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth = screenHeight * Screen.width / Screen.height;
        Debug.Log(Screen.width + " " + Screen.height);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        float spacingX = screenWidth / (gridSize);
        float spacingY = screenHeight / (gridSize);

        for (int x = 0; x < gridSize + 1; x++)
        {
            for (int y = 0; y < gridSize + 1; y++)
            {
                Vector3 pos = new Vector3((x * spacingX) - screenWidth/2 , (y * spacingY) - screenHeight/2, 0f);
                Gizmos.DrawSphere(pos, 0.08f);
            }
        }
    }

}
