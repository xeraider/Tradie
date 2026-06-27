using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField]
    private GridManger gridManager;

    [SerializeField]
    private GameObject connectionPrefab;

    //the currently selected Water Node
    private WaterNode selectedWaterNode;

    //list of existing connections
    private List<Connection> connections = new List<Connection>();

    /// <summary>
    /// Called to select the node from the touch input of a player.
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
    /// Called to select the connection from the touch input of a player.
    /// </summary>
    public void TapConnection(Connection c)
    {
        UpdateConnection(c);
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

        //checks if there are water nodes in the way of the connections
        if (IsPathBlockedByNode(a, b, orientation))
        {
            Debug.Log("Path blocked!");
            return;
        }

        //checks if there are water nodes in the way of the connections
        if (IsPathBlockedByConnection(a, b, orientation))
        {
            Debug.Log("Path blocked by connection!");
            return;
        }

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
            //sets the higher value to the right
            return (a.gridX > b.gridX) ? (b, a) : (a, b);
        }
        else
        {
            //sets the higher value to the right
            return (a.gridY > b.gridY) ? (b, a) : (a, b);
        }
    }

    /// <summary>
    /// Called to see if there is already a connection that exists.
    /// </summary>
    private bool ConnectionExists(WaterNode a, WaterNode b, Connection.Axis orientaion)
    {
        //iterates through a list of connections
        foreach (Connection c in connections) 
        {
            //if the connection contains the any of the nodes in the correct position and if orientation is the same
            if ((c.startWaterNode == a || c.endWaterNode == b) && orientaion == c.orientation)
            {
                //if the exact same connection exists
                if (c.startWaterNode == a && c.endWaterNode == b)
                {
                    UpdateConnection(c);
                }
                else
                {
                    Debug.Log("A connection for this water node face already exists!");
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Called to see if there are water nodes in the way of the connections.
    /// </summary>
    private bool IsPathBlockedByNode(WaterNode a, WaterNode b, Connection.Axis orientaion)
    {
        //iterates through a grid to check if path blocked
        if (orientaion == Connection.Axis.Horizontal)
        {
            for (int i = a.gridX + 1; i < b.gridX; i++)
            {
                if (gridManager.grid[i,a.gridY] != null)
                {
                    return true;
                }
            }
        }
        else if (orientaion == Connection.Axis.Vertical)
        {
            for (int i = a.gridY + 1; i < b.gridY; i++)
            {
                if (gridManager.grid[a.gridX, i] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Called to see if there are connections in the way of the connections.
    /// </summary>
    private bool IsPathBlockedByConnection(WaterNode a, WaterNode b, Connection.Axis orientaion)
    {
        //iterates through a existing connections to check if path blocked by connections
        if (orientaion == Connection.Axis.Horizontal)
        {
            foreach (Connection c in connections)
            {
                if (a.gridX < c.startWaterNode.gridX && c.startWaterNode.gridX < b.gridX && c.startWaterNode.gridY < a.gridY && a.gridY < c.endWaterNode.gridY)
                {
                    return true;
                }
            }
        }
        else if (orientaion == Connection.Axis.Vertical)
        {
            foreach (Connection c in connections)
            {
                if (a.gridY < c.startWaterNode.gridY && c.startWaterNode.gridY < b.gridY && c.startWaterNode.gridX < a.gridX && a.gridX < c.endWaterNode.gridX)
                {
                    return true;
                }
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

        Quaternion pipeOrientation;
        int pipeLength;

        //sets pipe orientation and length
        if (orientation == Connection.Axis.Vertical)
        {
            pipeOrientation = Quaternion.Euler(0, 0, 90);
            pipeLength = b.gridY - a.gridY;
        }
        else {
            pipeOrientation = Quaternion.identity;
            pipeLength = b.gridX - a.gridX;
        }

        Debug.Log(pipeLength);

        //creates connection
        GameObject connectionObject = Instantiate(connectionPrefab, pos, pipeOrientation);
        Connection c = connectionObject.GetComponent<Connection>();

        //Set up for connection
        c.Setup(a, b, orientation, pipeLength);

        return c;
    }

    /// <summary>
    /// Called to update an existing connection.
    /// </summary>
    private void UpdateConnection(Connection c)
    {
        //if connection is double then remove the connection
        if (c.doubleConnection)
        {
            Destroy(c.gameObject);
            connections.Remove(c);
        }
        //if connection is single then double the connection
        else
        {
            c.doubleConnection = true;
            c.UpdateSprite();
        }
    }
}
