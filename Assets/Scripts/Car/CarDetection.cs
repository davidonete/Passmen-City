using UnityEngine;
using System.Collections;

public class CarDetection : MonoBehaviour {

    public LayerMask LayerMask;

	// Use this for initialization
	void Start ()
  {
	
	}


  // Update is called once per frame
  void FixedUpdate()
  {
    RayCast();
  }

  void RayCast()
  {
    RaycastHit hit;
    //Debug.DrawLine(transform.position, transform.position + (transform.forward * 2.0f),Color.blue);
    if (Physics.Raycast(transform.position,transform.forward, out hit, 3.0f, LayerMask))
    {
      if (hit.collider.gameObject.tag == "Car")
      {
        Debug.DrawLine(transform.position, hit.transform.position, Color.green, 2.0f);
        Vector3 selfTargetDirection = transform.forward;
        Vector3 hitTargetDirection = hit.transform.forward;

        if (hitTargetDirection == selfTargetDirection)
           gameObject.GetComponent<CarBehaviour>().SetIsOtherCarNear(true);
         
      }
 
      if (hit.collider.gameObject.tag == "CrossWalk")
      {
        if (!gameObject.GetComponent<CarBehaviour>().GetCarStates.IsCrossing)
        {
          gameObject.GetComponent<CarBehaviour>().SetIsCrossWalkDetected(true);

          if (hit.collider.gameObject.GetComponent<CrossWalkBehaviour>().GetCrossWalkStates == CrossWalkBehaviour.CrossWalkStates.kCrossWalkStates_GreenLight)
            gameObject.GetComponent<CarBehaviour>().SetIsGreenLightOn(true);
          else
            gameObject.GetComponent<CarBehaviour>().SetIsGreenLightOn(false);
        }
      }
    }
    else
    {
      gameObject.GetComponent<CarBehaviour>().SetIsOtherCarNear(false);
      gameObject.GetComponent<CarBehaviour>().SetIsCrossWalkDetected(false);
    }
  }
}
