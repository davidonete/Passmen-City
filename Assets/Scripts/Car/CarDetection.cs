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

  /*void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Car" && (!other.GetComponent<CarBehaviour>().GetCarStates.IsNearObjective && !GetComponent<CarBehaviour>().GetCarStates.IsNearObjective))
    {
      Vector3 selfTargetDirection = transform.forward;
      Vector3 hitTargetDirection = other.transform.forward;

      if (hitTargetDirection == selfTargetDirection)
      {
        Debug.Log("Hola");
        if (GetComponent<CarBehaviour>().GetCarStates.DistanceFromObjective >= other.GetComponent<CarBehaviour>().GetCarStates.DistanceFromObjective)
          GetComponent<CarBehaviour>().SetIsOtherCarNear(true);
        else
         other.GetComponent<CarBehaviour>().SetIsOtherCarNear(true);
      }
    }
  }

  void OnTriggerExit(Collider other)
  {
    if (other.tag == "Car")
    {
      if (GetComponent<CarBehaviour>().GetCarStates.IsOtherCarNear)
        GetComponent<CarBehaviour>().SetIsOtherCarNear(false);
      else
        other.GetComponent<CarBehaviour>().SetIsOtherCarNear(false);
    }
  }*/

  void RayCast()
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position,transform.forward, out hit, 3.0f, LayerMask))
    {

      if (hit.collider.gameObject.tag == "Car")
      {
        Vector3 selfTargetDirection = transform.forward;
        Vector3 hitTargetDirection = hit.transform.forward;

        if (hitTargetDirection == selfTargetDirection)
           gameObject.GetComponent<CarBehaviour>().SetIsOtherCarNear(true);    
      }

      /*if (hit.collider.gameObject.tag == "Car" && (!hit.collider.GetComponent<CarBehaviour>().GetCarStates.IsNearObjective || !GetComponent<CarBehaviour>().GetCarStates.IsNearObjective))
      {
        Vector3 selfTargetDirection = transform.forward;
        Vector3 hitTargetDirection = hit.transform.forward;

        if (hitTargetDirection == selfTargetDirection)
        {
          Debug.DrawLine(transform.position, hit.transform.position, Color.green, 2.0f);

          if (hit.collider.GetComponent<CarBehaviour>().GetCarStates.DistanceFromObjective >= GetComponent<CarBehaviour>().GetCarStates.DistanceFromObjective)
            hit.collider.GetComponent<CarBehaviour>().SetIsOtherCarNear(true);
          else
            GetComponent<CarBehaviour>().SetIsOtherCarNear(true);
        }
      }*/
    }
    else
    {
      if(gameObject.GetComponent<CarBehaviour>().GetCarStates.IsOtherCarNear)
        gameObject.GetComponent<CarBehaviour>().SetIsOtherCarNear(false);
    }
  }
}
