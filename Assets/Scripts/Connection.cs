using System.Data;
using UnityEngine;

/// <summary>
/// Represents Connection.
/// </summary>
public class Connection : MonoBehaviour
{
    //enum for the state
    public enum State
    {
        Single,
        Double,
        None,
        Invalid
    }

    private State _connectionState;

    //enum for the direction
    public enum Axis
    {
        Horizontal,
        Vertical
    }
    public Axis Orientation { get; private set; }

    //length of the connection
    private int _connectionLength;

    //terminals
    public Terminal StartTerminal { get; set; }
    public Terminal EndTerminal { get; set; }


    //sprites
    [SerializeField]
    private Sprite singlePipe, doublePipe;

    /// <summary>
    /// Called to setup the connection and give it values.
    /// </summary>
    public void Setup(Terminal a, Terminal b, Axis o, int size)
    {
        StartTerminal = a;
        EndTerminal = b;
        Orientation = o;
        _connectionLength = size;

        UpdateSprite();
    }

    /// <summary>
    /// Called to update the sprite when doubled.
    /// </summary>
    public void UpdateSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        BoxCollider2D bc = GetComponent<BoxCollider2D>();

        if (ConnectionState == State.Double)
        {
            //change size and sprite to double pipe
            sr.sprite = doublePipe;
            sr.size = new Vector2(_connectionLength * 2 - 1, 0.5f);
            bc.size = new Vector2(_connectionLength * 2 - 1, 0.5f);
        }
        else if (ConnectionState == State.Single)
        {
            //change size and sprite to single pipe
            sr.sprite = singlePipe;
            sr.size = new Vector2(_connectionLength * 2 - 1, 0.25f);
            bc.size = new Vector2(_connectionLength * 2 - 1, 0.25f);
        }
    }

    public State ConnectionState
    {
        get => _connectionState;
        set
        {
            _connectionState = value;
            UpdateSprite();
        }
    }
}
