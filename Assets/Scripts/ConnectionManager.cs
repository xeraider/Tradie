using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    //The currently selected Water Node
    private WaterNode selectedWaterNode;

    /// <summary>
    /// Called to Select the node from the touch input of a player.
    /// </summary>
    public void SelectWaterNode(WaterNode targetWaterNode)
    {
        //If tapped on empty space then set the unselect the selected node
        if (targetWaterNode == null)
        {
            selectedWaterNode = null;
            return;
        }

        //If the selected node is the one being tapped on then do nothing
        if (selectedWaterNode == targetWaterNode)
        {
            return;
        }

        //if their is no selected node then set it to the tapped node
        if (selectedWaterNode == null)
        {
            selectedWaterNode = targetWaterNode;
            return;
        }

        //Creates the connection and resets the selected node.
        TryConnect(selectedWaterNode, targetWaterNode);
        selectedWaterNode = null;
    }

    /// <summary>
    /// Called to try connect two water nodes.
    /// </summary>
    public void TryConnect(WaterNode nodeA, WaterNode nodeB)
    {
        //Checks and makes sure the connections are not diagonal
        if (nodeA.transform.position.x != nodeB.transform.position.x && nodeA.transform.position.y != nodeB.transform.position.y)
        {
            Debug.Log("Pipes cannot connect diagonally");
            return;
        }

        //Checks the directions of the connection
        if (nodeA.transform.position.x != nodeB.transform.position.x)
        {
            //Checks left and right
            if (nodeA.transform.position.x > nodeB.transform.position.x)
            {
                if (nodeA.left != null || nodeB.right != null)
                {
                    Debug.Log("Connection already exits.");
                    return;
                }
                //Sets direction inside the nodes
                nodeA.left = nodeB;
                nodeB.right = nodeA;
            }
            else
            {
                if (nodeA.right != null || nodeB.left != null)
                {
                    Debug.Log("Connection already exits.");
                    return;
                }
                //Sets direction inside the nodes
                nodeA.right = nodeB;
                nodeB.left = nodeA;
            }
        }
        else
        {
            //Checks up and down
            if (nodeA.transform.position.y > nodeB.transform.position.y)
            {
                if (nodeA.down != null || nodeB.up != null)
                {
                    Debug.Log("Connection already exits.");
                    return;
                }
                //Sets direction inside the nodes
                nodeA.down = nodeB;
                nodeB.up = nodeA;
            }
            else
            {
                if (nodeA.up != null || nodeB.down != null)
                {
                    Debug.Log("Connection already exits.");
                    return;
                }
                //Sets direction inside the nodes
                nodeA.up = nodeB;
                nodeB.down = nodeA;
            }
        }
        //Draws line representaion.
        DrawLine(nodeA, nodeB);
        Debug.Log("Connected " + nodeA.name + " to " + nodeB.name);
    }

    /// <summary>
    /// Called to draw a line between two nodes to represent a connection
    /// </summary>
    private void DrawLine(WaterNode nodeA, WaterNode nodeB)
    {
        //Create game object for pipe
        GameObject pipe = new GameObject("Pipe");

        //Line renderer draws the line which 
        LineRenderer lr = pipe.AddComponent<LineRenderer>();

        lr.positionCount = 2;

        lr.SetPosition(0, new Vector2(nodeA.transform.position.x, nodeA.transform.position.y));
        lr.SetPosition(1, new Vector2(nodeB.transform.position.x, nodeB.transform.position.y));

        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.red;
        lr.endColor = Color.blue;
    }
}
