using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels/Level")]
public class LevelScriptableObject : ScriptableObject
{
    public int gridSize;
    public TerminalData[] terminalData;
}
