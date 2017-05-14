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
    Vector3 point = transform.position + transform.forward * 4.0f + new Vector3(0.0f, 0.5f, 0.0f);
    if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), transform.forward, out hit, 4.0f, LayerMask) && hit.collider != this)
    {
      if (hit.collider.gameObject.tag == "Car")
      {
        Vector3 selfTargetDirection = gameObject.transform.forward.normalized;
        Vector3 hitTargetDirection = hit.collider.gameObject.transform.forward.normalized;

        float angle = Vector3.Angle(hitTargetDirection, selfTargetDirection);

        if (angle <= 10.0f)
          gameObject.GetComponent<CarBehaviour>().SetIsOtherCarNear(true);
      }
    }
    else
    {
      if(gameObject.GetComponent<CarBehaviour>().GetCarStates.IsOtherCarNear)
        gameObject.GetComponent<CarBehaviour>().SetIsOtherCarNear(false);
    }
  }
}
