using UnityEngine;
using System.Collections;

public class TrafficLightBehaviour : MonoBehaviour
{
  private Renderer Renderer;
  private Material Material;
  private bool IsRed;
  public CrossWalkBehaviour CrossWalkReference;

  void Start()
  {
    Renderer = GetComponent<Renderer>();
    Material = Renderer.material;
    IsRed = true;
  }

  void Update()
  {
    // Green light
    if (CrossWalkReference.GetCrossWalkStates == CrossWalkBehaviour.CrossWalkStates.kCrossWalkStates_GreenLight)
    {
      if (!IsRed)
      {
        ChangeEmissiveColor(Color.green, Color.black);
        IsRed = true;
      }
    }
    // Red light
    else
    {
      if (IsRed)
      {
        ChangeEmissiveColor(Color.black, Color.red);
        IsRed = false;
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
