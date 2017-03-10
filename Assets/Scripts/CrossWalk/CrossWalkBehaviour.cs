using UnityEngine;
using System.Collections;

public class CrossWalkBehaviour : MonoBehaviour {

  public enum CrossWalkStates
  {
    kCrossWalkStates_GreenLight,
    kCrossWalkStates_RedLight
  }

  private struct CrossWalkConditions
  {
    // Both States
    public float TimeBetweenChanges;

    // RedLight State
    public bool IsPedestrianWaiting;
  }

  private CrossWalkStates State;
  private CrossWalkConditions Condition;
  public float TimeBetweenChanges;

  // Use this for initialization
  void Start ()
  {
    State = CrossWalkStates.kCrossWalkStates_RedLight;

    Condition.TimeBetweenChanges = TimeBetweenChanges;
    Condition.IsPedestrianWaiting = false;
  }
	
	// Update is called once per frame
	void Update ()
  {
    StateMachine();
  }

  void StateMachine()
  {
    switch (State)
    {
      case CrossWalkStates.kCrossWalkStates_RedLight:
        RedLight();
        break;

      case CrossWalkStates.kCrossWalkStates_GreenLight:
        GreenLight();
        break;

      default:
        break;
    }
  }

  void RedLight()
  {
    if (Condition.IsPedestrianWaiting)
    {
      if (Condition.TimeBetweenChanges <= 0.0f)
      {
        Condition.TimeBetweenChanges = TimeBetweenChanges;
        State = CrossWalkStates.kCrossWalkStates_GreenLight;
      }
      else
        Timer();
    }
  }

  void GreenLight()
  {
    if (Condition.TimeBetweenChanges <= 0.0f)
    {
      Condition.TimeBetweenChanges = TimeBetweenChanges;
      State = CrossWalkStates.kCrossWalkStates_RedLight;
    }
    else
      Timer();
  }

  void Timer()
  {
    if(Condition.TimeBetweenChanges > 0.0f)
      Condition.TimeBetweenChanges -= Time.deltaTime;
  }


  // Setters & Getters

  public CrossWalkStates GetState()
  {
    return State;
  }

  public void SetIsPedestrianWaiting(bool result)
  {
    Condition.IsPedestrianWaiting = result;
  }

  public bool GetIsPedestrianWaiting()
  {
    return Condition.IsPedestrianWaiting;
  }
}
