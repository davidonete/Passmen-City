using UnityEngine;
using System.Collections.Generic;
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
    public List<GameObject> NumberOfPedestrians;
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
    Condition.NumberOfPedestrians = new List<GameObject>();
  }

    // Update is called once per frame
  void Update ()
  {
    StateMachine();
    CheckDeadPedestrians();
  }

  private void FixedUpdate()
  {
    if (State == CrossWalkStates.kCrossWalkStates_RedLight)
      Debug.DrawRay(transform.position, Vector3.up, Color.red);
    else if (State == CrossWalkStates.kCrossWalkStates_GreenLight)
      Debug.DrawRay(transform.position, Vector3.up, Color.green);
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
      if (Condition.NumberOfPedestrians.Count == 0)
      {
        Condition.TimeBetweenChanges = TimeBetweenChanges;
        Condition.IsPedestrianWaiting = false;
        State = CrossWalkStates.kCrossWalkStates_RedLight;
      }
    }
    else
      Timer();
  }

  void CheckDeadPedestrians()
  {
    for (int i = Condition.NumberOfPedestrians.Count - 1; i >= 0; i--)
    {
      if (Condition.NumberOfPedestrians.Contains(Condition.NumberOfPedestrians[i]))
      {
        if (Condition.NumberOfPedestrians[i] != null)
        {
          if(Condition.NumberOfPedestrians[i].gameObject.GetComponent<PedestrianBehavior>().GetPedestrianState == PedestrianBehavior.PedestrianState.kPedestrianState_Dead)
            Condition.NumberOfPedestrians.Remove(Condition.NumberOfPedestrians[i]);
        }
      }
    }
  }

  void Timer()
  {
    if(Condition.TimeBetweenChanges > 0.0f)
      Condition.TimeBetweenChanges -= Time.deltaTime;
  }

  void OnTriggerEnter(Collider other)
  {
    if(other.gameObject.tag == "Pedestrian")
    {
      if (!Condition.NumberOfPedestrians.Contains(other.gameObject))
        Condition.NumberOfPedestrians.Add(other.gameObject);
    }

  }

  void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag == "Pedestrian")
    {
      if (Condition.NumberOfPedestrians.Contains(other.gameObject))
        Condition.NumberOfPedestrians.Remove(other.gameObject);
    }
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
}
