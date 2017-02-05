using UnityEngine;
using System.Collections;

/// <summary>
/// Segment
/// Represents a connection between two nodes.
/// </summary>
public class Segment : MonoBehaviour
{
    public Node NodeA;
    public Node NodeB;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(NodeA.Position, NodeB.Position);
    }
}
