using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Connection
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
    public void Setup(WaterNode a, WaterNode b, Axis orientation)
    {
        startWaterNode = a;
        endWaterNode = b;
        axisDirection = direction;
    }

    /// <summary>
    /// Called to draw a line between two nodes to represent a connection
    /// </summary>
    public void Draw()
    {
        GameObject pipe = new GameObject("Pipe");

        LineRenderer lr = pipe.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, startWaterNode.transform.position);
        lr.SetPosition(1, endWaterNode.transform.position);

        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.red;
        lr.endColor = Color.blue;
    }
}
