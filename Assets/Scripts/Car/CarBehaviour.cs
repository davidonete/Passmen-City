using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CarBehaviour : MonoBehaviour {

  private enum CarStates
  {
    kCarState_Searching,
    kCarState_Driving,
    kCarState_Waiting
  }

  public struct CarConditions
  {
    // Searching State
    public bool IsSearching;
    // Driving State
    public bool IsDriving;
    public bool IsGreenLightOn;
    public bool IsCrossWalkDetected;
    public bool IsReachedPoint;
    public bool IsOtherCarNear;
    // Waiting State
    public bool IsWaiting;
  }
 
  private CarStates State;
  private CarConditions Condition;

  private bool IsInitialized = false;

  private List<Vector3> Path = new List<Vector3>();
  private Vector3 NextLocation;
  public float MinDistance;
  public float Speed;

  private Rigidbody RB;

  void Start()
  {
    Init();
  }

  void Init()
  {
    //this.CarStateas
    State = CarStates.kCarState_Driving;

    Condition.IsSearching = true;
    Condition.IsDriving = false;
    Condition.IsGreenLightOn = false;
    Condition.IsCrossWalkDetected = false;
    Condition.IsReachedPoint = false;
    Condition.IsWaiting = false;
    Condition.IsOtherCarNear = false;

    RB = GetComponent<Rigidbody>();

    IsInitialized = true;
  }

  void Update()
  {
    if (IsInitialized)
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
      GetNewPath();
      GetNextLocationStep();
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
    /*if (Condition.IsDriving)
    { 
      if (Condition.IsCrossWalkDetected && Condition.IsGreenLightOn)
      {
        Condition.IsDriving = false;
        Condition.IsWaiting = true;
        State = CarStates.kCarState_Waiting;
      }
      else
      {
        // Test
        if (!Condition.IsOtherCarNear)
          PathfindingMovement();
      }
    }
    else
    {
      Condition.IsSearching = true;
      State = CarStates.kCarState_Searching;
    }*/
    RB.MovePosition(transform.position + (transform.forward * Time.deltaTime * Speed));


  }

  // Waiting traffic light
  void Waiting()
  {
    if (Condition.IsWaiting)
    {
      if (!Condition.IsGreenLightOn)
        Condition.IsWaiting = false;
    }
    else
    {
      Condition.IsDriving = true;
      State = CarStates.kCarState_Driving;
    }
  }

  ///  PathFinding

  void GetNewPath()
  {
    Vector3 start = AStarSearch.GetNearestWaypoint(WaypointsExample.CarsGraph, transform.position);
    Vector3 end = AStarSearch.GetRandomWaypoint(WaypointsExample.CarsGraph);
    Path = AStarSearch.FindNewObjective(WaypointsExample.PedestriansGraph, start, end);
  }

  bool GetNextLocationStep()
  {
    if (Path.Count > 0)
    {
      NextLocation = Path[Path.Count - 1];
      Path.RemoveAt(Path.Count - 1);

      return true;
    }
    return false;
  }

  void PathfindingMovement()
  {
    if (Vector3.Distance(transform.position, NextLocation) > MinDistance)
    {
      transform.position = Vector3.MoveTowards(transform.position, NextLocation, Time.deltaTime * Speed);
      transform.forward = (NextLocation - transform.position).normalized;
    }
    else
    {
      transform.position = NextLocation;
      if (!GetNextLocationStep())
        Condition.IsDriving = false;
    }
  }

  // Setters & Getters

  public void SetIsGreenLightOn(bool result)
  {
    Condition.IsGreenLightOn = result;
  }

  public void SetIsCrossWalkDetected(bool result)
  {
    Condition.IsCrossWalkDetected = result;
  }

  public void SetIsOtherCarNear(bool result)
  {
    Condition.IsOtherCarNear = result;
  }

  public CarConditions GetCarStates
  {
    get { return Condition; }
  }
}
