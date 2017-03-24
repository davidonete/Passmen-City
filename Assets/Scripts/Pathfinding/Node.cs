using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// NodeNeighbors
/// A neighbor node and its distance to the initial node
/// </summary>
[System.Serializable]
public class NodeNeighbors
{
  public GameObject OtherNode;
  public float Distance;
}

/// <summary>
/// Node 
/// 3D position
/// </summary>
public class Node : MonoBehaviour
{
    public Color DebugColor;
    public float DebugSize = 0.75f;

    // A list of nodes conected to this node
    [SerializeField]
    private List<NodeNeighbors> mNeighbors = new List<NodeNeighbors>();
  
    public List<NodeNeighbors> Neighbors
    {
        get { return mNeighbors; }
        set { mNeighbors = value; }
    }

    //Add a node to the list of neighbors
    public void AddNeighbor(NodeNeighbors node)
    {
        mNeighbors.Add(node);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = DebugColor;
        Gizmos.DrawSphere(transform.position, DebugSize);
        foreach (NodeNeighbors go in mNeighbors)
        {
            Gizmos.DrawLine(transform.position, go.OtherNode.transform.position);
        }
    }
}