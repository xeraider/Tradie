
using System.Collections.Generic;

/// <summary>
/// Handles connection validation.
/// </summary>
public class ConnectionRules
{
    //list of all connections
    public List<Connection> AllConnections;

    //terminal grid
    private Terminal[,] _terminalGrid;

    /// <summary>
    /// Called to set connection validator to the terminal grid.
    /// </summary>
    public void Set(Terminal[,] TerminalGrid)
    {
        _terminalGrid = TerminalGrid;
        AllConnections = new List<Connection>();
    }

    /// <summary>
    /// Called to validate connection.
    /// </summary>
    public Connection.State Validate(Terminal a, Terminal b, out Terminal StartTerminal, out Terminal EndTerminal, out Connection.Axis Orientation, out Connection ExistingConnection)
    {
        Connection.Axis o;

        //checks orientation
        o = GetOrientation(a, b);

        //orders the terminals according to position (a is smaller than b)
        (a, b) = OrderTerminals(a, b, o);

        StartTerminal = a;
        EndTerminal = b;
        Orientation = o;
        ExistingConnection = null;

        //checks if path is blocked by terminals
        if (IsPathBlockedByTerminal(a, b, o)) return Connection.State.Invalid;

        //checks if path is blocked by conenctions
        if (IsPathBlockedByConnection(a, b, o)) return Connection.State.Invalid;

        //checks if conenction exists
        ExistingConnection = ConnectionExists(a, b, o);
        if (ExistingConnection == null) return Connection.State.None;

        //returns existing connection
        return ExistingConnection.ConnectionState;
    }

    /// <summary>
    /// Called to get the orientation of a connection.
    /// </summary>
    private Connection.Axis GetOrientation(Terminal a, Terminal b)
    {
        if (a.GridX != b.GridX)
        {
            return Connection.Axis.Horizontal;
        }
        else
        {
            return Connection.Axis.Vertical;
        }
    }

    /// <summary>
    /// Called to order the water terminals according to direction.
    /// </summary>
    private (Terminal, Terminal) OrderTerminals(Terminal a, Terminal b, Connection.Axis o)
    {
        //horizontal or vertical
        if (o == Connection.Axis.Horizontal)
        {
            //sets the higher value to the right
            return (a.GridX > b.GridX) ? (b, a) : (a, b);
        }
        else
        {
            //sets the higher value to the right
            return (a.GridY > b.GridY) ? (b, a) : (a, b);
        }
    }

    /// <summary>
    /// Called to see if there are terminals in the way of the connections.
    /// </summary>
    private bool IsPathBlockedByTerminal(Terminal a, Terminal b, Connection.Axis o)
    {
        //iterates through a grid to check if path blocked
        if (o == Connection.Axis.Horizontal)
        {
            for (int i = a.GridX + 1; i < b.GridX; i++)
            {
                if (_terminalGrid[i, a.GridY] != null)
                {
                    return true;
                }
            }
        }
        else if (o == Connection.Axis.Vertical)
        {
            for (int i = a.GridY + 1; i < b.GridY; i++)
            {
                if (_terminalGrid[a.GridX, i] != null)
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
    private bool IsPathBlockedByConnection(Terminal a, Terminal b, Connection.Axis o)
    {
        //iterates through a existing connections to check if path blocked by connections
        foreach (Connection c in AllConnections)
        {
            if (c.Orientation == o)
                continue;

            if (o == Connection.Axis.Horizontal)
            {
                return (a.GridX < c.StartTerminal.GridX && c.StartTerminal.GridX < b.GridX && c.StartTerminal.GridY < a.GridY && a.GridY < c.EndTerminal.GridY);
            }
            else
            {
                return (a.GridY < c.StartTerminal.GridY && c.StartTerminal.GridY < b.GridY && c.StartTerminal.GridX < a.GridX && a.GridX < c.EndTerminal.GridX);
            }
        }
        return false;
    }

    /// <summary>
    /// Called to see if there is already a connection that exists.
    /// </summary>
    private Connection ConnectionExists(Terminal a, Terminal b, Connection.Axis o)
    {
        //checks if connection exists
        if (o == Connection.Axis.Horizontal)
        {
            if (a.Connections[1] != null || b.Connections[3] != null) 
            {
                if (a.Connections[1] == b.Connections[3])
                {
                    return a.Connections[1];
                }
            } 
        }
        else if (o == Connection.Axis.Vertical)
        {
            if (a.Connections[0] != null || b.Connections[2] != null)
            {
                if (a.Connections[0] == b.Connections[2])
                {
                    return a.Connections[0];
                }
            }
        }
        return null;
    }
}
