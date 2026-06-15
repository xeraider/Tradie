using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    //The currently selected Water Node
    private WaterNode selectedWaterNode;

    //The currently forming connection
    private Connection currConnection;

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
        //connection that is being formed
        currConnection = new Connection();

        //sets the direction for the current connection
        currConnection.orientation = GetOrientaion(a, b);

        //orders the nodes according to the direction
        (currConnection.startWaterNode, currConnection.endWaterNode) = OrderWaterNodes(a, b, currConnection.orientation);

        //checks if connection already exists on the face of the water node
        if (ConnectionExists(currConnection.startWaterNode, currConnection.endWaterNode, currConnection.orientation))
        {
            return;
        }

        //creates and adds connection to the list and resets currentconnection
        connections.Add(currConnection);
        currConnection.Draw();
        Debug.Log("Connected " + a.name + " to " + b.name);
        currConnection = null;
    }

    /// <summary>
    /// Called to get the orientation of a connection.
    /// </summary>
    private Connection.Axis GetOrientaion(WaterNode a, WaterNode b)
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
        foreach (Connection c in connections) 
        {
            if ((c.startWaterNode == a || c.endWaterNode == b) && direction == c.orientation)
            {
                if (c.startWaterNode == a && c.endWaterNode == b && !c.doubleConnection)
                {
                    c.doubleConnection = true;
                    c.Draw();
                    Debug.Log("Double pipe!");
                }
                Debug.Log("Connection already exists!");
                return true;
            }
        }
        return false;
    }
}
