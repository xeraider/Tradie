using UnityEngine;

/// <summary>
/// Handles levels.
/// </summary>
public class LevelManager : MonoBehaviour
{
    //levels for the game
    [SerializeField]
    private LevelScriptableObject[] _levels;

    //camera
    [SerializeField]
    private CameraManager _cameraManager;

    //terminal manager containing terminal information
    [SerializeField]
    private TerminalManager _terminalManager;

    //connection manager containing connection information
    [SerializeField]
    private ConnectionManager _connectionManager;

    //level that is currently in play
    public int CurrentLevel { get; private set;}

    /// <summary>
    /// Called to initialise the level.
    /// </summary>
    public void InitialiseLevel(int level)
    {
        //return if level is out of bounds of the levels
        if (level < 0 || level >= _levels.Length) return;

        //clear current grid
        _terminalManager.ClearGrid();

        //sets camera
        _cameraManager.SetCamera(_levels[level].gridSize);

        //loads level grid
        _terminalManager.PopulateGrid(_levels[level]);

        //sets the validator to the current level
        _connectionManager.SetValidator();

        //sets level to current level
        CurrentLevel = level;
    }
}
