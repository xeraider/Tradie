using UnityEngine;

/// <summary>
/// Handles Game.
/// </summary>
public class GameManager : MonoBehaviour
{
    //manages the levels
    [SerializeField]
    private LevelManager _levelManager;

    //manages the connections
    [SerializeField]
    private ConnectionManager _connectionManager;

    //manages the camera
    [SerializeField]
    private CameraManager _cameraManager;

    //the status of the game 
    public enum GameState
    {
        Loading,
        Playing,
        Paused,
        LevelComplete
    }

    public GameState State { get; private set; }

    private void Start()
    {
        LoadLevel(0);
    }

    private void OnEnable()
    {
        // start listening to the connection event
        ConnectionManager.OnConnection += CheckWin;
    }

    private void OnDisable()
    {
        // stop listening to the connection event
        ConnectionManager.OnConnection -= CheckWin;
    }

    /// <summary>
    /// Called to load Level.
    /// </summary>
    private void LoadLevel(int level)
    {
        State = GameState.Loading;

        _levelManager.InitialiseLevel(level);

        State = GameState.Playing;
    }

    /// <summary>
    /// Called to restart level.
    /// </summary>
    private void RestartLevel() {
        Time.timeScale = 1.0f;
        LoadLevel(_levelManager.CurrentLevel);
    }

    /// <summary>
    /// Called to go to the next level.
    /// </summary>
    private void NextLevel()
    {
        Time.timeScale = 1.0f;
        LoadLevel(_levelManager.CurrentLevel + 1);
    }

    /// <summary>
    /// Called to go to the previous level.
    /// </summary>
    private void PreviousLevel()
    {
        Time.timeScale = 1.0f;
        LoadLevel(_levelManager.CurrentLevel - 1);
    }

    /// <summary>
    /// Called to go to the pause game.
    /// </summary>
    private void PauseGame()
    {
        if (State != GameState.Playing) return;

        State = GameState.Paused;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Called to go to the resume game.
    /// </summary>
    private void Resume()
    {
        if (State != GameState.Paused) return;

        State = GameState.Playing;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Called to go to check the win.
    /// </summary>
    public void CheckWin()
    {
        if (State != GameState.Playing) return;

        Debug.Log("Checking win...");

        if (!_connectionManager.CheckWin()) return;

        State = GameState.LevelComplete;

        Debug.Log("Level Complete!");

        NextLevel();
    }
}
