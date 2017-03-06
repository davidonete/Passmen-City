using UnityEngine;
using System.Collections;

public class CrossWalkDetection : MonoBehaviour {

  public TrafficLightBehaviour TrafficLightReference;

  private GameObject CarReference;
  private bool IsCarWaiting;

  // Use this for initialization
  void Start ()
  {
    CarReference = null;
    IsCarWaiting = false;
  }
	
	// Update is called once per frame
	void Update ()
  {
    CarWaiting();
	}

  void OnCollisionEnter(Collision car)
  {
    if(car.gameObject.tag == "Car")
    {
      car.gameObject.GetComponent<CarBehaviour>().SetIsObstacleDetected(true);

      if (TrafficLightReference.GetTrafficLightState() == TrafficLightBehaviour.TrafficLightState.kTrafficLightStates_Red)
      {
        car.gameObject.GetComponent<CarBehaviour>().SetIsGreenLightOn(false);
        CarReference = car.gameObject;
        IsCarWaiting = true;
      }
      else
        car.gameObject.GetComponent<CarBehaviour>().SetIsGreenLightOn(true);
    }
  }

  void OnCollisionExit(Collision car)
  {
    if (car.gameObject.tag == "Car")
    {
      car.gameObject.GetComponent<CarBehaviour>().SetIsObstacleDetected(false);
    }
  }

  void CarWaiting()
  {
    if (IsCarWaiting)
    {
      if (TrafficLightReference.GetTrafficLightState() == TrafficLightBehaviour.TrafficLightState.kTrafficLightStates_Green)
      {
        CarReference.GetComponent<CarBehaviour>().SetIsGreenLightOn(true);
        IsCarWaiting = false;
      }
    }
  }
}
