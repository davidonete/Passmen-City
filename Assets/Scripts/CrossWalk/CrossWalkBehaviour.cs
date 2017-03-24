using UnityEngine;
using System.Collections;

public class CrossWalkBehaviour : MonoBehaviour {

  public enum CrossWalkStates
  {
    kCrossWalkStates_GreenLight,
    kCrossWalkStates_RedLight
  }

  public struct CrossWalkConditions
  {
    // Both States
    public float TimeBetweenChanges;

    // RedLight State
    public bool IsPedestrianWaiting;
    // GreenLight State
    public int NumberOfPedestriansCrossing;
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
      if (Condition.NumberOfPedestriansCrossing == 0)
      {
        Condition.TimeBetweenChanges = TimeBetweenChanges;
        Condition.IsPedestrianWaiting = false;
        State = CrossWalkStates.kCrossWalkStates_RedLight;
      }
    }
    else
      Timer();
  }

  void Timer()
  {
    if(Condition.TimeBetweenChanges > 0.0f)
      Condition.TimeBetweenChanges -= Time.deltaTime;
  }

  void OnTriggerEnter(Collider other)
  {
     if(other.gameObject.tag == "Pedestrian")
       Condition.NumberOfPedestriansCrossing++;
  }

  void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag == "Pedestrian")
      Condition.NumberOfPedestriansCrossing--;
  }


    // Setters & Getters

  public void SetIsPedestrianWaiting(bool result)
  {
    Condition.IsPedestrianWaiting = result;
  }

  public CrossWalkStates GetCrossWalkStates
  {
    get { return State; }
    set { State = value; }
  }

  public CrossWalkConditions GetCrossWalkConditions
  {
    get { return Condition; }
    set { Condition = value; }
  }

    /* private bool mPaco;
     public bool Paco
     {
         get { return mPaco; }
         set { mPaco = value; }
     }

     this.Paco = true;
     bool a = this.Paco;*/
}
