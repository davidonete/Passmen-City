using UnityEngine;
using System.Collections;

/// <summary>
/// Node 
/// 3D position
/// </summary>
public class Node : MonoBehaviour
{
    // The position of the node in the scene
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    // A list of segments conected to this node
    // NOTE:the array is not initialized by default.
    private Segment[] mSegments;
    public Segment[] Segments
    {
        get { return mSegments; }
        set { mSegments = value; }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.75f);
    }
}
