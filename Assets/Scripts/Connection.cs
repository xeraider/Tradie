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

    /// <summary>
    /// Called to setup the connection and give it values.
    /// </summary>
    public void Setup(WaterNode a, WaterNode b, Axis o)
    {
        startWaterNode = a;
        endWaterNode = b;
        orientation = o;
    }

    public bool DoubleConnection
    {
        get => doubleConnection;
        set
        {
            doubleConnection = value;
            Double();
        }
    }

    /// <summary>
    /// Called to update the sprite when doubled.
    /// </summary>
    public void Double()
    {
        Vector3 scale = this.transform.localScale;

        if (orientation == Connection.Axis.Horizontal)
        {
            scale.y = this.doubleConnection ? startWaterNode.transform.localScale.y * 0.4f : startWaterNode.transform.localScale.y * 0.2f;
        }
        if (orientation == Connection.Axis.Vertical)
        {
            scale.x = this.doubleConnection ? startWaterNode.transform.localScale.x * 0.4f : startWaterNode.transform.localScale.x * 0.2f;
        }

        this.transform.localScale = scale;
    }
}
