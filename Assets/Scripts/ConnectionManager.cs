using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField]
    private GridManger gridManager;

    [SerializeField]
    private GameObject connectionPrefab;

    //the currently selected terminal
    private Terminal selectedTerminal;

    //list of existing connections
    private List<Connection> connections = new List<Connection>();

    /// <summary>
    /// Called to select the terminal from the touch input of a player.
    /// </summary>
    public void SelectTerminal(Terminal targetTerminal)
    {
        //If tapped on empty space then set the unselect the selected terminal
        if (targetTerminal == null)
        {
            if (selectedTerminal) selectedTerminal.IsSelected = false;
            selectedTerminal = null;
            return;
        }

        //If the selected terminal is the one being tapped on then do nothing
        if (selectedTerminal == targetTerminal)
        {
            return;
        }

        //if their is no selected terminal then set it to the tapped terminal
        if (selectedTerminal == null)
        {
            selectedTerminal = targetTerminal;
            selectedTerminal.IsSelected = true;
            return;
        }

        //Checks and makes sure the connections are not diagonal
        if (selectedTerminal.gridX != targetTerminal.gridX && selectedTerminal.gridY != targetTerminal.gridY)
        {
            selectedTerminal.IsSelected = false;
            selectedTerminal = null;
            return;
        }

        //Creates the connection and resets the selected terminal.
        TryConnect(selectedTerminal, targetTerminal);
        selectedTerminal.IsSelected = false;
        selectedTerminal = null;
    }

    /// <summary>
    /// Called to select the connection from the touch input of a player.
    /// </summary>
    public void TapConnection(Connection c)
    {
        UpdateConnection(c);
        if (selectedTerminal) selectedTerminal.IsSelected = false;
        selectedTerminal = null;
    }

    /// <summary>
    /// Called to try connect two water terminals. The connection will always go from the negative to the positive terminal position.
    /// </summary>
    public void TryConnect(Terminal a, Terminal b)
    {
        //orientation of the connection
        Connection.Axis orientation = GetOrientation(a, b);

        //connecting water terminals that have been ordered by position
        (Terminal, Terminal) orderedTerminals = OrderTerminals(a, b, orientation);
        a = orderedTerminals.Item1;
        b = orderedTerminals.Item2;

        //checks if there are water terminals in the way of the connections
        if (IsPathBlockedByTerminal(a, b, orientation)) return;

        //checks if there are water terminals in the way of the connections
        if (IsPathBlockedByConnection(a, b, orientation)) return;

        //checks if connection already exists on the faces of the water terminal
        if (ConnectionExists(a, b, orientation)) return;

        //add connection to the existing connections list
        Connection c = InitialiseConnection(a, b, orientation);
        connections.Add(c);
    }

    /// <summary>
    /// Called to get the orientation of a connection.
    /// </summary>
    private Connection.Axis GetOrientation(Terminal a, Terminal b)
    {
        if (a.gridX != b.gridX)
        {
            return Connection.Axis.Horizontal;
        }
        return Connection.Axis.Vertical;
    }

    /// <summary>
    /// Called to order the water terminals according to direction.
    /// </summary>
    private (Terminal, Terminal) OrderTerminals(Terminal a, Terminal b, Connection.Axis orientaion)
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
    private bool ConnectionExists(Terminal a, Terminal b, Connection.Axis orientaion)
    {
        //iterates through a list of connections
        foreach (Connection c in connections) 
        {
            //if the connection contains the any of the terminals in the correct position and if orientation is the same
            if ((c.startTerminal == a || c.endTerminal == b) && orientaion == c.orientation)
            {
                //if the exact same connection exists
                if (c.startTerminal == a && c.endTerminal == b)
                {
                    UpdateConnection(c);
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Called to see if there are water terminals in the way of the connections.
    /// </summary>
    private bool IsPathBlockedByTerminal(Terminal a, Terminal b, Connection.Axis orientaion)
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
    private bool IsPathBlockedByConnection(Terminal a, Terminal b, Connection.Axis orientaion)
    {
        //iterates through a existing connections to check if path blocked by connections
        if (orientaion == Connection.Axis.Horizontal)
        {
            foreach (Connection c in connections)
            {
                if (a.gridX < c.startTerminal.gridX && c.startTerminal.gridX < b.gridX && c.startTerminal.gridY < a.gridY && a.gridY < c.endTerminal.gridY)
                {
                    return true;
                }
            }
        }
        else if (orientaion == Connection.Axis.Vertical)
        {
            foreach (Connection c in connections)
            {
                if (a.gridY < c.startTerminal.gridY && c.startTerminal.gridY < b.gridY && c.startTerminal.gridX < a.gridX && a.gridX < c.endTerminal.gridX)
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
    private Connection InitialiseConnection(Terminal a, Terminal b, Connection.Axis orientation)
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
