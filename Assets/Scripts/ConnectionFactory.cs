using UnityEngine;

/// <summary>
/// Handles connection creation.
/// </summary>
public class ConnectionFactory : MonoBehaviour {

    /// <summary>
    /// Called to initialise the connection
    /// </summary>
    public Connection InitialiseConnection(Terminal a, Terminal b, Connection.Axis orientation, GameObject prefab)
    {
        //position of the connection
        Vector2 pos = (a.transform.position + b.transform.position) / 2f;

        Quaternion pipeOrientation;
        int pipeLength;

        //sets pipe orientation and length
        if (orientation == Connection.Axis.Vertical)
        {
            pipeOrientation = Quaternion.Euler(0, 0, 90);
            pipeLength = b.GridY - a.GridY;
        }
        else
        {
            pipeOrientation = Quaternion.identity;
            pipeLength = b.GridX - a.GridX;
        }


        //updates the current connections
        a.CurrentConnections += 1;
        b.CurrentConnections += 1;

        //creates connection
        GameObject connectionObject = GameObject.Instantiate(prefab, pos, pipeOrientation);
        Connection c = connectionObject.GetComponent<Connection>();

        //adds connections to the terminals
        if (orientation == Connection.Axis.Horizontal)
        {
            a.Connections[1] = c;
            b.Connections[3] = c;
        }
        else
        {
            a.Connections[0] = c;
            b.Connections[2] = c;
        }

        //Set up for connection
        c.Setup(a, b, orientation, pipeLength);
        return c;
    }

    /// <summary>
    /// Called to make a single connection a double.
    /// </summary>
    public void DoubleConnection(Connection c)
    {
        //change connection state to double
        c.ConnectionState = Connection.State.Double;

        //increment connection count of terminal
        c.StartTerminal.CurrentConnections += 1;
        c.EndTerminal.CurrentConnections += 1;
    }

    /// <summary>
    /// Called to remove connection.
    /// </summary>
    public void RemoveConnection(Connection c)
    {
        //removes connection from terminals
        if (c.Orientation == Connection.Axis.Horizontal) 
        {
            c.StartTerminal.Connections[1] = null;
            c.StartTerminal.Connections[3] = null;
        }
        else
        {
            c.StartTerminal.Connections[0] = null;
            c.StartTerminal.Connections[2] = null;
        }

        //decrement connection count of terminal
        c.StartTerminal.CurrentConnections -= 2;
        c.EndTerminal.CurrentConnections -= 2;

        //destroys connection gameobject
        Destroy(c.gameObject);
    }
}
