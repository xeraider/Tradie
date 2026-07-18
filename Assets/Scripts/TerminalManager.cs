using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the grid of terminals.
/// </summary>
public class TerminalManager : MonoBehaviour
{
    //terminal prefab
    [SerializeField]
    private GameObject _terminalPrefab;

    //grid of terminals
    public int GridSize {  get; private set; }
    public Terminal[,] Grid { get; private set; }

    //first terminal for a DFS search
    private Terminal _firstTerminal;

    //number of terminals in grid
    private int _terminalCount;

    //terminal spacing
    private float _spacingX = 2f, _spacingY = 2f;

    /// <summary>
    /// Called to instantiate the Grid of terminals.
    /// </summary>
    public void PopulateGrid(LevelScriptableObject level)
    {
        bool isFirst = true;

        _terminalCount = level.terminalData.Length;

        //initialises grid
        GridSize = level.gridSize;
        Grid = new Terminal[GridSize,GridSize];

        foreach (TerminalData t in level.terminalData)
        {
            int gridX = t.gridX, gridY = t.gridY;

            //position of the terminal
            Vector2 pos = new Vector2((gridX - (GridSize - 1) / 2f) * _spacingX, (gridY - (GridSize - 1) / 2f) * _spacingY);

            //creates the terminal
            GameObject terminalObject = Instantiate(_terminalPrefab, pos, Quaternion.identity);
            Terminal terminal = terminalObject.GetComponent<Terminal>();
            terminal.SetUp(gridX, gridY, t.connectionLimit);

            //sets the initial first terminal
            if (isFirst) 
            {
                _firstTerminal = terminal;

                isFirst = false;
            }
            
            //add terminal to grid
            Grid[gridX, gridY] = terminal;
        }
    }

    /// <summary>
    /// Called to clear the Grid of terminals.
    /// </summary>
    public void ClearGrid()
    {
        //if there is no grid then return
        if (Grid == null) return;

        //destroys all terminal game objects
        foreach (Terminal t in Grid)
        {
            if(t != null)
            {
                Destroy(t.gameObject);
            }
        }

        //clears Grid
        Grid = null;
    }

    /// <summary>
    /// Called to check if all terminals are connected to each other.
    /// </summary>
    public bool AreAllTerminalsConnected()
    {
        //skip there are no terminals
        if (_firstTerminal == null || _terminalCount == 0)
            return false;

        HashSet<Terminal> visited = new();

        //checks if all terminals are connected
        Traverse(_firstTerminal, visited);
        return visited.Count == _terminalCount;
    }

    /// <summary>
    /// Called to traverse terminal grid.
    /// </summary>
    private void Traverse(Terminal terminal, HashSet<Terminal> visited)
    {
        //back track if visited already
        if (visited.Contains(terminal)) return;

        //adds the terminal to the visited list
        visited.Add(terminal);

        //checks all connections in terminal to see if they are connected to another terminal
        foreach (Connection connection in terminal.Connections)
        {
            //skip null connections
            if (connection == null)
                continue;

            //traverses to the other terminal
            Terminal next = connection.StartTerminal == terminal ? connection.EndTerminal : connection.StartTerminal;
            Traverse(next, visited);
        }
    }
}
