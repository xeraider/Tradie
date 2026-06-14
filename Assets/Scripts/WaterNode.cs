using System.Collections.Generic;
using UnityEngine;

public class WaterNode : MonoBehaviour
{
    //Connections 
    public List<WaterNode> pipeConnections = new List<WaterNode>();

    private void Awake()
    {
    }
}
