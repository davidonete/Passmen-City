using UnityEngine;
using System.Collections;

public class NeighbourDetector : MonoBehaviour
{
    PedestrianBehavior mParent;

	void Awake ()
    {
        mParent = this.transform.parent.GetComponent<PedestrianBehavior>();
    }
	
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.layer == 8)
            mParent.AddNeighbour(Other.gameObject);
    }

    void OnTriggerExit(Collider Other)
    {
        if (Other.gameObject.layer == 8)
            mParent.RemoveNeighbour(Other.gameObject);
    }
}
