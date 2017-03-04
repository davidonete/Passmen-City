using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {


  private enum CarStates
  {
    kCarState_Searching,
    kCarState_Driving,
    kCarState_Waiting
  }



  private struct CarConditions
  {
    // Searching State
    public bool IsSearching;
    // Driving State
    public bool IsDriving;
    public bool IsGreenLightOn;
    public bool IsObstacleDetected;
    public bool IsReachedPoint;
    // Waiting State
    public bool IsWaiting;
  }
 
  private CarStates State;
  private CarConditions Condition;

  // Test
  public bool IsMovingLeft;

  void Start()
  {
    State = CarStates.kCarState_Searching;

    Condition.IsSearching = true;
    Condition.IsDriving = false;
    Condition.IsGreenLightOn = false;
    Condition.IsObstacleDetected = false;
    Condition.IsReachedPoint = false;
    Condition.IsWaiting = false;
  }

  void Update()
  {
    StateMachine();
  }

  void StateMachine()
  {
    switch (State)
    {
      case CarStates.kCarState_Searching:
        Searching();
        break;

      case CarStates.kCarState_Driving:
        Driving();
        break;

      case CarStates.kCarState_Waiting:
        Waiting();
        break;

      default:
        break;
    }
  }

  // Searching for a new direction
  void Searching()
  {
    if (Condition.IsSearching)
    {
      /*
      if (SearchedPoint)
      {
        Condition.IsSearching = false;
      }
      */
      Condition.IsSearching = false;
    }
    else
    {
      Condition.IsDriving = true;
      State = CarStates.kCarState_Driving;
    }
  }

  // Driving to the end point
  void Driving()
  {
    if (Condition.IsDriving)
    { 
      if (Condition.IsObstacleDetected && !Condition.IsGreenLightOn)
      {
        Condition.IsDriving = false;
        Condition.IsWaiting = true;
        State = CarStates.kCarState_Waiting;
      }
      else
      {
        // Test
        if (IsMovingLeft)
        {
          transform.Translate(Vector3.down * 1.0f * Time.deltaTime);
        }
        else
        {
          transform.Translate(Vector3.up * 1.0f * Time.deltaTime);
        }
      }

      /*
     if (ReachedPoint)
     {
       Condition.IsDriving = false;
     }
     */
    }
    else
    {
      Condition.IsSearching = true;
      State = CarStates.kCarState_Searching;
    }
  }

  // Waiting traffic light
  void Waiting()
  {
    if (Condition.IsWaiting)
    {
      if (Condition.IsGreenLightOn)
        Condition.IsWaiting = false;
    }
    else
    {
      Condition.IsDriving = true;
      State = CarStates.kCarState_Driving;
    }
  }

   public void SetIsGreenLightOn(bool result)
  {
    Condition.IsGreenLightOn = result;
  }

  public void SetIsObstacleDetected(bool result)
  {
    Condition.IsObstacleDetected = result;
  }
}
