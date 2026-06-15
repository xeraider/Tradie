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
    public void Setup(WaterNode a, WaterNode b, Axis o)
    {
        startWaterNode = a;
        endWaterNode = b;
        orientation = o;
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
        lr.sortingOrder = 0;

        if (!doubleConnection) 
        {
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.startColor = Color.blue;
            lr.endColor = Color.blue;
        }
        else 
        {
            lr.startWidth = 0.3f;
            lr.endWidth = 0.3f;
            lr.startColor = Color.red;
            lr.endColor = Color.red;
        }
        
        lr.material = new Material(Shader.Find("Sprites/Default"));
        
    }
}
