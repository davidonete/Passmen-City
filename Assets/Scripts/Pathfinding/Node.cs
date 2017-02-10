using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Node 
/// 3D position
/// </summary>
public class Node : MonoBehaviour
{
    //Set the node walcable or not
    private bool mWalcable = true;

    // A list of nodes conected to this node
    [SerializeField]
    private List<GameObject> mNeighbors = new List<GameObject>();
    
    public bool Walcable
    {
        get { return mWalcable; }
        set { mWalcable = value; }
    }

    public List<GameObject> Neighbors
    {
        get { return mNeighbors; }
        set { mNeighbors = value; }
    }

    //Add a node to the list of neighbors
    public void AddNeighbor (GameObject node)
    {
        mNeighbors.Add(node);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.75f);
    }
}
