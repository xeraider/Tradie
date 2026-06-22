using UnityEngine;
using UnityEngine.UIElements;

public class Connection : MonoBehaviour
{
    //Enum for the direction
    public enum Axis
    {
        Horizontal,
        Vertical
    }

    //Direction of the connection
    public Axis orientation;

    //Connections with start being negative to end being positive
    public WaterNode startWaterNode, endWaterNode;

    //Connection count
    public bool doubleConnection;

    private SpriteRenderer sr;

    //Sprites
    [SerializeField]
    private Sprite singlePipe, doublePipe;

    void Awake()
    {
        sr = this.GetComponent<SpriteRenderer>();
        sr.sprite = singlePipe;
    }

    /// <summary>
    /// Called to setup the connection and give it values.
    /// </summary>
    public void Setup(WaterNode a, WaterNode b, Axis o)
    {
        startWaterNode = a;
        endWaterNode = b;
        orientation = o;
        this.name = a.name + "-" + b.name;
    }

    /// <summary>
    /// Called to update the sprite when doubled.
    /// </summary>
    public void Double()
    {
        doubleConnection = true;

        sr = this.GetComponent<SpriteRenderer>();
        sr.size = new Vector2(sr.size.x, 0.5f);

        sr.sprite = doublePipe;

        //sets new name and changes the width of the connection according to the double connection boolean
        this.name = startWaterNode.name + "=" + startWaterNode.name;
    }
}
