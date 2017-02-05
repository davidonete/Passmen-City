using UnityEngine;
using System.Collections;

/// <summary>
/// Node 
/// 3D position
/// </summary>
public class Node : MonoBehaviour
{
    // The position of the node in the scene
    private Vector3 mPosition;
    public Vector3 Position
    {
        get { return mPosition; }
        set { mPosition = value; }
    }

    // A list of segments conected to this node
    // NOTE:the array is not initialized by default.
    private Segment[] mSegments;
    public Segment[] Segments
    {
        get { return mSegments; }
        set { mSegments = value; }
    }
}
