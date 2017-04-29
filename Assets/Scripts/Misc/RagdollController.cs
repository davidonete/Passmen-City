using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour
{
    public bool DisabledOnStart = true;
    public Transform Spine;

    void Start()
    {
        if(DisabledOnStart) SetKinematic(true);
    }

    /// <summary>
    /// Sets recursively the kinematic state of the parent and 
    /// childs
    /// </summary>
    /// <param name="kinematic"> kinematic state </param>
    public void SetKinematic(bool kinematic)
    {
        SetKinematicImp(Spine, kinematic);
    }

    void SetKinematicImp(Transform rb,bool kinematic)
    {
        // Sets the current rb if possible
        var curRb = rb.GetComponent<Rigidbody>();
        if (curRb) curRb.isKinematic = kinematic;

        // Iterates the childs
        for (var i=0;i<rb.childCount;i++)
        {
            SetKinematicImp(rb.GetChild(i), kinematic);
        }
    }
}
