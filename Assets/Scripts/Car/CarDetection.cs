using UnityEngine;
using System.Collections;

public class CarDetection : MonoBehaviour {

	// Use this for initialization
	void Start ()
  {
	
	}

  // Update is called once per frame
  void Update()
  {
    RayCast();
  }

  void RayCast()
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.0f))
    {
      if (hit.collider.gameObject.tag == "Car")
        gameObject.GetComponent<CarBehaviour>().SetIsOtherCarNear(true);

      if (hit.collider.gameObject.tag == "CrossWalk")
      {
        gameObject.GetComponent<CarBehaviour>().SetIsCrossWalkDetected(true);

        if (hit.collider.gameObject.GetComponent<CrossWalkBehaviour>().GetCrossWalkStates == CrossWalkBehaviour.CrossWalkStates.kCrossWalkStates_GreenLight)
          gameObject.GetComponent<CarBehaviour>().SetIsGreenLightOn(true);
        else
          gameObject.GetComponent<CarBehaviour>().SetIsGreenLightOn(false);
      }
    }
    else
    {
      gameObject.GetComponent<CarBehaviour>().SetIsOtherCarNear(false);
      gameObject.GetComponent<CarBehaviour>().SetIsCrossWalkDetected(false);
    }
  }
}
