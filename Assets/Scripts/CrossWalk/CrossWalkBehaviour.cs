﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CrossWalkBehaviour : MonoBehaviour {

  private List<GameObject> TriggerContainer = new List<GameObject>();

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
      if (Condition.NumberOfPedestriansCrossing <= 0)
      {
        Condition.TimeBetweenChanges = TimeBetweenChanges;
        Condition.IsPedestrianWaiting = false;
        State = CrossWalkStates.kCrossWalkStates_RedLight;
      }
      else
      {
        //*-CheckDeadPedestrians();
      }
    }
    else
      Timer();
  }

  void CheckDeadPedestrians()
  {
    for (int i = TriggerContainer.Count - 1; i >= 0; i--)
    {
      if (TriggerContainer.Contains(TriggerContainer[i]))
      {
        if (TriggerContainer[i].gameObject.GetComponent<PedestrianBehavior>().GetPedestrianState == PedestrianBehavior.PedestrianState.kPedestrianState_Dead)
        {
          TriggerContainer.Remove(TriggerContainer[i]);
          Condition.NumberOfPedestriansCrossing--;
          break;
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
      if (!TriggerContainer.Contains(other.gameObject))
      {
        TriggerContainer.Add(other.gameObject);
        Condition.NumberOfPedestriansCrossing++;
      }
    }

  }

  void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag == "Pedestrian")
    {
      if (TriggerContainer.Contains(other.gameObject))
      {
        TriggerContainer.Remove(other.gameObject);
        Condition.NumberOfPedestriansCrossing--;
      }
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
