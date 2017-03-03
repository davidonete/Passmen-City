using UnityEngine;
using System.Collections;

public class TrafficLight : MonoBehaviour {

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

  // Use this for initialization
  void Start () {
    SaveTimebetweenChanges = TimebetweenChanges;

    Renderer = GetComponent<Renderer>();
    RandomMaterial();
  }
	
	// Update is called once per frame
  void Update () {
    Timer();
  }

  public TrafficLightState GetTrafficLightState()
  {
    return State;
  }

  void RandomMaterial()
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

  void Timer()
  {
    TimebetweenChanges -= Time.deltaTime;
    if (TimebetweenChanges < 0)
    {
      ChangeMaterial();
      TimebetweenChanges = SaveTimebetweenChanges;
    }
  }

  void ChangeMaterial()
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
}
