using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject connectionPrefab;

    //The currently selected Water Node
    private WaterNode selectedWaterNode;

    //List of existing connections
    private List<Connection> connections = new List<Connection>();

    /// <summary>
    /// Called to Select the node from the touch input of a player.
    /// </summary>
    public void SelectWaterNode(WaterNode targetWaterNode)
    {
        //If tapped on empty space then set the unselect the selected node
        if (targetWaterNode == null)
        {
            selectedWaterNode = null;
            return;
        }

        //If the selected node is the one being tapped on then do nothing
        if (selectedWaterNode == targetWaterNode)
        {
            return;
        }

        //if their is no selected node then set it to the tapped node
        if (selectedWaterNode == null)
        {
            selectedWaterNode = targetWaterNode;
            return;
        }

        //Checks and makes sure the connections are not diagonal
        if (selectedWaterNode.gridX != targetWaterNode.gridX && selectedWaterNode.gridY != targetWaterNode.gridY)
        {
            selectedWaterNode = null;
            return;
        }

        //Creates the connection and resets the selected node.
        TryConnect(selectedWaterNode, targetWaterNode);
        selectedWaterNode = null;
    }

    /// <summary>
    /// Called to try connect two water nodes. The connection will always go from the negative to the positive node position.
    /// </summary>
    public void TryConnect(WaterNode a, WaterNode b)
    {
        //orientation of the connection
        Connection.Axis orientation = GetOrientation(a, b);

        //connecting water nodes that have been ordered by position
        (WaterNode, WaterNode) orderedWaterNodes = OrderWaterNodes(a, b, orientation);
        a = orderedWaterNodes.Item1;
        b = orderedWaterNodes.Item2;

        //checks if connection already exists on the faces of the water node
        if (ConnectionExists(a, b, orientation))
        {
            return;
        }

        //add connection to the existing connections list
        Connection c = InitialiseConnection(a, b, orientation);
        connections.Add(c);
        Debug.Log("Connected " + a.name + " to " + b.name);
    }

    /// <summary>
    /// Called to get the orientation of a connection.
    /// </summary>
    private Connection.Axis GetOrientation(WaterNode a, WaterNode b)
    {
        if (a.gridX != b.gridX)
        {
            return Connection.Axis.Horizontal;
        }
        return Connection.Axis.Vertical;
    }

    /// <summary>
    /// Called to order the water nodes according to direction.
    /// </summary>
    private (WaterNode, WaterNode) OrderWaterNodes(WaterNode a, WaterNode b, Connection.Axis orientaion)
    {
        //horizontal or vertical
        if (orientaion == Connection.Axis.Horizontal) 
        {
            //sets the higher value to a
            return (a.gridX > b.gridX) ? (b, a) : (a, b);
        }
        else
        {
            //sets the higher value to a
            return (a.gridY > b.gridY) ? (b, a) : (a, b);
        }
    }

    /// <summary>
    /// Called to see if there is already a connection that exists.
    /// </summary>
    private bool ConnectionExists(WaterNode a, WaterNode b, Connection.Axis direction)
    {
        //iterates through a list to show
        foreach (Connection c in connections) 
        {
            if ((c.startWaterNode == a || c.endWaterNode == b) && direction == c.orientation)
            {
                if (c.startWaterNode == a && c.endWaterNode == b && !c.doubleConnection)
                {
                    c.DoubleConnection = true;
                    Debug.Log("Double pipes!");
                }
                else
                {
                    Debug.Log("Connection already exists!");
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Called to initialise the connection
    /// </summary>
    private Connection InitialiseConnection(WaterNode a, WaterNode b, Connection.Axis orientation)
    {
        //position of the connection
        Vector2 pos = (a.transform.position + b.transform.position) / 2f;

        //creates connection
        GameObject connectionObject = Instantiate(connectionPrefab, pos, Quaternion.identity);
        Connection c = connectionObject.GetComponent<Connection>();

        //Set up for connection
        c.Setup(a, b, orientation);

        //gets the curent scale of the connection
        Vector3 scale = c.transform.localScale;
        scale.z = 0;

        //sets the size according to the orientation and if its double or single pipe
        if (orientation == Connection.Axis.Horizontal)
        {
            scale.x = Mathf.Abs(a.transform.position.x - b.transform.position.x) - a.transform.localScale.x;
            scale.y = a.transform.localScale.y * 0.2f;
        }
        if (orientation == Connection.Axis.Vertical)
        {
            scale.y = Mathf.Abs(a.transform.position.y - b.transform.position.y) - a.transform.localScale.y;
            scale.x = a.transform.localScale.x * 0.2f;
            
        }
        c.transform.localScale = scale;

        return c;
    }
}
