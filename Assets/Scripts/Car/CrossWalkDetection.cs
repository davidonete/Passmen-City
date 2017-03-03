using UnityEngine;
using System.Collections;

public class CrossWalkDetection : MonoBehaviour {

  private TrafficLight TrafficLightInstance;
  private bool CanPass;

  // Use this for initialization
  void Start () {
    GameObject tl = GameObject.Find("TrafficLight");
    TrafficLightInstance = tl.GetComponent<TrafficLight>();
  }
	
	// Update is called once per frame
	void Update () {
	
	}

  public bool GetCanPass()
  {
    return CanPass;
  }

  void OnCollisionEnter(Collision collider)
  {
    if(collider.gameObject.tag == "Car")
    {
      if (TrafficLightInstance.GetTrafficLightState() == TrafficLight.TrafficLightState.kTrafficLightStates_Red)
        CanPass = false;
      else
        CanPass = true;
    }
  }
}
