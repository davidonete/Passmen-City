using UnityEngine;
using System.Collections;

public class TrafficLightBehaviour : MonoBehaviour
{
  private Renderer Renderer;
  private Material Material;
  private bool IsOpen;
  private bool IsTrafficLigtForCars;
  public CrossWalkBehaviour CrossWalkReference;

  void Start()
  {
    Renderer = GetComponent<Renderer>();
    Material = Renderer.material;

    if (transform.parent.name == "CarTrafficLight")
      IsTrafficLigtForCars = true;
    else
      IsTrafficLigtForCars = false;

    IsOpen = true;
  }

  void Update()
  {
    // Green light
    if (CrossWalkReference.GetCrossWalkStates == CrossWalkBehaviour.CrossWalkStates.kCrossWalkStates_GreenLight)
    {
      if (!IsOpen)
      {
        if (!IsTrafficLigtForCars)
          ChangeEmissiveColor(Color.green, Color.black);
        else
          ChangeEmissiveColor(Color.black, Color.red);

        IsOpen = true;
      }
    }
    // Red light
    else
    {
      if (IsOpen)
      {
        if (!IsTrafficLigtForCars)
          ChangeEmissiveColor(Color.black, Color.red);
        else
          ChangeEmissiveColor(Color.green, Color.black);

        IsOpen = false;
      }
    }
  }

  void ChangeEmissiveColor(Color GreenLight, Color RedLight)
  {
    if (gameObject.tag == "GreenTrafficLight")
      Material.SetColor("_EmissionColor", GreenLight);

    if (gameObject.tag == "RedTrafficLight")
      Material.SetColor("_EmissionColor", RedLight);
  }
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
