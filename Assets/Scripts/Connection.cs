using UnityEngine;

public class Connection : MonoBehaviour
{
    //enum for the direction
    public enum Axis
    {
        Horizontal,
        Vertical
    }

    //direction of the connection
    public Axis orientation;

    //connections with start being negative to end being positive
    public WaterNode startWaterNode, endWaterNode;

    //length of the connection
    private int connectionLength;

    //connection count
    public bool doubleConnection;

    //sprites
    [SerializeField]
    private Sprite singlePipe, doublePipe;

    /// <summary>
    /// Called to setup the connection and give it values.
    /// </summary>
    public void Setup(WaterNode a, WaterNode b, Axis o, int size)
    {
        startWaterNode = a;
        endWaterNode = b;
        orientation = o;
        connectionLength = size;

        UpdateSprite();
    }

    /// <summary>
    /// Called to update the sprite when doubled.
    /// </summary>
    public void UpdateSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        BoxCollider2D bc = GetComponent<BoxCollider2D>();

        if (doubleConnection)
        {
            //change size and sprite to double pipe
            sr.size = new Vector2(connectionLength * 2 - 1, 0.5f);
            bc.size = new Vector2(connectionLength * 2 - 1, 0.5f);
            sr.sprite = doublePipe;

            //sets the name for debugging
            name = startWaterNode.name + "=" + startWaterNode.name;
        }
        else 
        {
            //change size and sprite to single pipe
            sr.size = new Vector2(connectionLength * 2 - 1, 0.25f);
            bc.size = new Vector2(connectionLength * 2 - 1, 0.25f);
            sr.sprite = singlePipe;

            //sets the name for debugging
            name = startWaterNode.name + "-" + startWaterNode.name;
        }
    }
}
