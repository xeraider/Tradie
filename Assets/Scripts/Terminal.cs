using UnityEngine;

public class Terminal : MonoBehaviour
{
    //grid position
    public int gridX, gridY;

    //the number of connections the terminal can connect to
    public int connectionLimit;

    private bool isSelected;

    //sprites
    [SerializeField]
    private Sprite[] sprites;

    /// <summary>
    /// Called to setup the terminal and give it values.
    /// </summary>
    public void SetUp(int x, int y, int ConnectionLimit)
    {
        //sets variaables for gridPosition and the connection limit
        gridX = x;
        gridY = y;
        connectionLimit = Mathf.Clamp(ConnectionLimit, 1, 8);

        name = "Terminal(" + x + "," + y + ")";

        //updates the sprite according to ConnectionLimit
        UpdateSprite();
    }

    public bool IsSelected {
        get => isSelected;
        set
        {
            isSelected = value;
            UpdateSprite();
        }
    }

    /// <summary>
    /// Called to update sprite.
    /// </summary>
    public void UpdateSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (isSelected) 
        {
            //sets selected sprite 
            sr.sprite = sprites[connectionLimit + 8];
            return;
        }

        //sets sprite 
        sr.sprite = sprites[connectionLimit - 1];
    }
}

