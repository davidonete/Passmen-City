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

    public void EnableRagdoll()
    {
        if (!mEnabled)
        {
            SetKinematicImp(Spine, false);
            mEnabled = true;
        }
    }

    public void DisableRagdoll()
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
        // Disable colliders
        var curBoxCol = rb.GetComponent<BoxCollider>();
        if(curBoxCol)
        {
            curBoxCol.enabled = !kinematic;
        }
        else
        {
            var curCapsuleCol = rb.GetComponent<CapsuleCollider>();
            if(curCapsuleCol)
            {
                curCapsuleCol.enabled = !kinematic;
            }
        }
        

        // Iterates the childs
        for (var i=0;i<rb.childCount;i++)
        {
            SetKinematicImp(rb.GetChild(i), kinematic);
        }
    }
}
