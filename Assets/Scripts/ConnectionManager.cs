
using System;
using UnityEngine;

/// <summary>
/// Handles Connections.
/// </summary>
public class ConnectionManager : MonoBehaviour
{
    //prefab for connection
    [SerializeField]
    private GameObject _connectionPrefab;

    //terminal manager
    [SerializeField]
    private TerminalManager _terminalManager;

    //factory that makes connections
    [SerializeField]
    private ConnectionFactory _factory;

    //validation for connections
    private ConnectionRules _checker = new ConnectionRules();

    //event for when a connection is made
    public static event Action OnConnection;

    //the currently selected terminal
    private Terminal _selectedTerminal;

    /// <summary>
    /// Called to set the connection manager.
    /// </summary>
    public void SetValidator()
    {
        if(_checker.AllConnections != null) 
        { 
            foreach (Connection c in _checker.AllConnections)
            {
                _factory.RemoveConnection(c);
            }

            _checker.AllConnections.Clear();
        }

        _checker.Set(_terminalManager.Grid);
    }

    /// <summary>
    /// Called to select the terminal from the touch input of a player.
    /// </summary>
    public void SelectTerminal(Terminal t)
    {
        //checks null terminal
        if (t == null) 
        {
            SelectedTerminal = null;
            return; 
        }

        //sets selected terminal if there is nothing selected
        if (SelectedTerminal == null)
        {
            SelectedTerminal = t;
            return;
        }

        //checks if connection is diagonal
        if (SelectedTerminal.GridX != t.GridX && SelectedTerminal.GridY != t.GridY)
        {
            SelectedTerminal = t;
            return;
        }

        //checks if same terminal is selected
        if (SelectedTerminal == t)
        {
            return;
        }

        //creates the connection and resets the selected terminal
        TryConnect(t);

        //unselect terminal
        SelectedTerminal = null;
        
    }

    /// <summary>
    /// Called to select the connection from the touch input of a player.
    /// </summary>
    public void SelectConnection(Connection c)
    {
        //changes selected connection according to the state
        switch (c.ConnectionState)
        {
            //doubles the connection
            case Connection.State.Single:
                _factory.DoubleConnection(c);
                OnConnection?.Invoke();
                break;
            //removes the connection
            case Connection.State.Double:
                _checker.AllConnections.Remove(c);
                _factory.RemoveConnection(c);
                break;
        }

        //unselect terminal
        SelectedTerminal = null;
    }

    /// <summary>
    /// Called to try connect the terminals.
    /// </summary>
    private void TryConnect(Terminal t)
    {
        //gets the status of the connection if there is one
        Connection.State connectionStatus = _checker.Validate(_selectedTerminal, t, out Terminal startTerminal, out Terminal endTerminal, out Connection.Axis orientation, out Connection ExistingConnection);

        switch (connectionStatus)
        {
            //skip due to invalid connection
            case Connection.State.Invalid:
                break;
            //create a connection
            case Connection.State.None:
                _checker.AllConnections.Add(_factory.InitialiseConnection(startTerminal, endTerminal, orientation, _connectionPrefab));
                OnConnection?.Invoke();
                break;
            //doubles the connection
            case Connection.State.Single:
                _factory.DoubleConnection(ExistingConnection);
                OnConnection?.Invoke();
                break;
            //removes the connection
            case Connection.State.Double:
                _checker.AllConnections.Remove(ExistingConnection);
                _factory.RemoveConnection(ExistingConnection);
                break;
        }
    }

    /// <summary>
    /// Called to check the win conditions.
    /// </summary>
    public bool CheckWin()
    {
        //checks to see if every terminal has the correct amount of connections.
        foreach (Terminal t in _terminalManager.Grid)
        {
            if (t == null) continue;
            if (t.CurrentConnections != t.MaxConnections) return false;
        }

        //checks to see if terminals is interconnected.
        return _terminalManager.AreAllTerminalsConnected();
    }

    private Terminal SelectedTerminal
    {
        get => _selectedTerminal;
        set
        {
            if (_selectedTerminal != null)
            {
                _selectedTerminal.Selected = false;
            }

            _selectedTerminal = value;

            if (_selectedTerminal != null)
            {
                _selectedTerminal.Selected = true;
            }
        }
    }
}
