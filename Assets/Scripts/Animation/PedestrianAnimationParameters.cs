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
    CheckParameters();
  }

  void CheckParameters()
  {
    if (Pedestrian.GetPedestrianState == PedestrianBehavior.PedestrianState.kPedestrianState_Walking)
      ModifyParameter(true, false, false);
    else if(Pedestrian.GetPedestrianState == PedestrianBehavior.PedestrianState.kPedestrianState_Waiting)
      ModifyParameter(false, true, false);
    else if (Pedestrian.GetPedestrianState == PedestrianBehavior.PedestrianState.kPedestrianState_Searching)
      ModifyParameter(false, false, true);
  }

  void ModifyParameter(bool walking, bool waiting, bool searching)
  {
    Animator.SetBool("IsWalking", walking);
    Animator.SetBool("IsWaiting", waiting);
    Animator.SetBool("IsSearching", searching);
  }
}
