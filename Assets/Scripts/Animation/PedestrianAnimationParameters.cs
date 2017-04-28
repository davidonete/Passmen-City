using UnityEngine;
using System.Collections;

public class PedestrianAnimationParameters : MonoBehaviour {

    private Animator Animator;
    private PedestrianBehavior Pedestrian;

	// Use this for initialization
	void Start ()
    {
      Animator = GetComponent<Animator>();
      Pedestrian = gameObject.GetComponent<PedestrianBehavior>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        ChangeParameters();
    }

    void ChangeParameters()
    {
        if (Pedestrian.GetPedestrianState == PedestrianBehavior.PedestrianState.kPedestrianState_Walking)
        {
            Animator.SetBool("IsWalking", true);
            Animator.SetBool("IsWaiting", false);
        }
        else if(Pedestrian.GetPedestrianState == PedestrianBehavior.PedestrianState.kPedestrianState_Waiting)
        {
            Animator.SetBool("IsWaiting", true);
            Animator.SetBool("IsWalking", false);
        }
    }
}
