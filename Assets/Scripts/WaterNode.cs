using UnityEngine;

public class WaterNode : MonoBehaviour
{
    //Grid position
    public int gridX, gridY;

    /// <summary>
    /// Called to setup the water node and give it values.
    /// </summary>
    public void SetUp(int x, int y)
    {
        gridX = x;
        gridY = y;
        this.name = "WaterNode(" + x + "," + y + ")";
    }
}

