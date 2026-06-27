using UnityEngine;

public class WaterNode : MonoBehaviour
{
    //grid position
    public int gridX, gridY;

    //the number of connections the node can connect to
    public int connectionLimit;

    //sprites
    [SerializeField]
    private Sprite[] sprites;

    /// <summary>
    /// Called to setup the water node and give it values.
    /// </summary>
    public void SetUp(int x, int y, int ConnectionLimit)
    {
        //sets variatables for gridPosition and the connection limit
        gridX = x;
        gridY = y;
        connectionLimit = Mathf.Clamp(ConnectionLimit, 1, 8);

        name = "WaterNode(" + x + "," + y + ")";

        //updates the sprite according to ConnectionLimit
        UpdateSprite();
    }

    /// <summary>
    /// Called to update sprite.
    /// </summary>
    public void UpdateSprite()
    {
        //sets sprite 
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[connectionLimit - 1];
    }
}

