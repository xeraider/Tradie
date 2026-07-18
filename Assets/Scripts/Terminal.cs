using UnityEngine;

/// <summary>
/// Represents a terminal.
/// </summary>
public class Terminal : MonoBehaviour
{
    //sprites
    [SerializeField]
    private Sprite[] sprites;

    //grid position
    public int GridX { get; private set; }
    public int GridY { get; private set; }

    //connections of terminal from up "0" going clockwise
    public Connection[] Connections = new Connection[4]; 

    //the number of connections the terminal can connect to
    private int _currentConnections, _maxConnections;

    //selected terminal
    private bool _selected;

    /// <summary>
    /// Called to setup the terminal and give it values.
    /// </summary>
    public void SetUp(int x, int y, int MaxConnections)
    {
        //sets variaables for gridPosition and the connection limit
        GridX = x;
        GridY = y;
        _maxConnections = MaxConnections;

        name = "Terminal(" + x + "," + y + ")";

        //updates the sprite according to ConnectionLimit
        UpdateSprite();
    }

    /// <summary>
    /// Called to update sprite.
    /// </summary>
    public void UpdateSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        //sets sprite 
        if (_selected) sr.sprite = sprites[_maxConnections + 8];
        else sr.sprite = sprites[_maxConnections - 1];
    }

    public int CurrentConnections
    {
        get => _currentConnections;
        set => _currentConnections = Mathf.Clamp(value, 0, 8);
    }

    public int MaxConnections
    {
        get => _maxConnections;
    }

    public bool Selected
    {
        get => _selected;
        set 
        {
            _selected = value;
            UpdateSprite() ;
        }
    }
}

