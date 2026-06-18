using UnityEngine;

public class WaterNode : MonoBehaviour
{
    //Grid position
    public int gridX, gridY;

    public void SetUp(string name, int x, int y)
    {
        this.name = name;
        gridX = x;
        gridY = y;
    }
}

