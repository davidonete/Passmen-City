using UnityEngine;
using System.Collections;

public class CarRayCast : MonoBehaviour {

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
    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.3f))
    {
      if (hit.collider.gameObject.tag == "Car")
        gameObject.GetComponent<CarBehaviour>().SetIsOtherCarNear(true);
    }
    else
      gameObject.GetComponent<CarBehaviour>().SetIsOtherCarNear(false);
  }
}
