using UnityEngine;
using System.Collections;

public class TrafficLightBehaviour : MonoBehaviour {

  public enum TrafficLightState
  {
    kTrafficLightStates_Green,
    kTrafficLightStates_Red
  }

  private TrafficLightState State;

  public Material Red;
  public Material Green;
  private Renderer Renderer;

  public float TimebetweenChanges;
  private float SaveTimebetweenChanges;

 
  void Start ()
  {
    SaveTimebetweenChanges = TimebetweenChanges;

    Renderer = GetComponent<Renderer>();
    RandomState();
  }

  void RandomState()
  {
    int number = Random.Range(0, 2);

    if (number == 0)
    {
      Renderer.material = Red;
      State = TrafficLightState.kTrafficLightStates_Red;
    }
    else
    {
      Renderer.material = Green;
      State = TrafficLightState.kTrafficLightStates_Green;
    }
  }

  void Update ()
  {
    StateMachine();
  }

  void StateMachine()
  {
    switch (State)
    {
      case TrafficLightState.kTrafficLightStates_Green:
        Timer();
        break;

      case TrafficLightState.kTrafficLightStates_Red:
        Timer();
        break;

      default:
        break;
    }
  }

  void Timer()
  {
    TimebetweenChanges -= Time.deltaTime;
    if (TimebetweenChanges < 0)
    {
      ChangeState();
      TimebetweenChanges = SaveTimebetweenChanges;
    }
  }

  void ChangeState()
  {
    if (Renderer.material.name.Substring(0, Renderer.material.name.Length - 11) == Red.name)
    {
      Renderer.material = Green;
      State = TrafficLightState.kTrafficLightStates_Green;
    }
    else
    {
      Renderer.material = Red;
      State = TrafficLightState.kTrafficLightStates_Red;
    }
  }

  public TrafficLightState GetTrafficLightState()
  {
    return State;
  }
}
