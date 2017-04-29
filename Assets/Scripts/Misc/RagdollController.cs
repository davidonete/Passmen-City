using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour
{
    public Transform Spine;
    private bool mEnabled = true;

    void Start()
    {
        DisableRagdoll();
    }

    void EnableRagdoll()
    {
        if (!mEnabled)
        {
            SetKinematicImp(Spine, false);
            mEnabled = true;
        }
    }

    void DisableRagdoll()
    {
        if(mEnabled)
        {
            SetKinematicImp(Spine, true);
            mEnabled = false;
        }
    }

    void SetKinematicImp(Transform rb,bool kinematic)
    {
        // Sets the current rb if possible
        var curRb = rb.GetComponent<Rigidbody>();
        if (curRb)
        {
            curRb.isKinematic = kinematic;
            curRb.detectCollisions = !kinematic;
        }

        // Iterates the childs
        for (var i=0;i<rb.childCount;i++)
        {
            SetKinematicImp(rb.GetChild(i), kinematic);
        }
    }
}
