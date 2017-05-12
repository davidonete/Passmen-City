using UnityEngine;
using System.Collections;

public class TrafficLightBehaviour : MonoBehaviour {

  public Material RedMaterial;
  public Material GreenMaterial;
  private Renderer Renderer;

  public CrossWalkBehaviour CrossWalkReference;
  //public Light TrafficLight;

  void Start ()
  {
    Renderer = GetComponent<Renderer>();
    //SelectMaterial();
  }

  void Update ()
  {
    //SelectLight();
    SelectMaterial();
  }

  /*void SelectLight()
  {
    if (CrossWalkReference.gameObject.GetComponent<CrossWalkBehaviour>().GetCrossWalkStates == CrossWalkBehaviour.CrossWalkStates.kCrossWalkStates_GreenLight)
    {
      if (gameObject.tag == "PedestrianTrafficLight")
        TrafficLight.color = Color.green;
      else
        TrafficLight.color = Color.red;
    }
    else
      if (gameObject.tag == "PedestrianTrafficLight")
        TrafficLight.color = Color.red;
      else
        TrafficLight.color = Color.green;
  }*/

  void SelectMaterial()
  {
    if (CrossWalkReference.gameObject.GetComponent<CrossWalkBehaviour>().GetCrossWalkStates == CrossWalkBehaviour.CrossWalkStates.kCrossWalkStates_GreenLight)
    {
      if (gameObject.tag == "PedestrianTrafficLight")
       ChangeMaterial(GreenMaterial);
      else
       ChangeMaterial(RedMaterial);
    }
    else
    {
      if (gameObject.tag == "PedestrianTrafficLight")
        ChangeMaterial(RedMaterial);
      else
        ChangeMaterial(GreenMaterial);
    }
  }

  void ChangeMaterial(Material material)
  {
      Renderer.material = material;
      Renderer.material.SetColor("_EmissionColor", material.color);
  }

}
