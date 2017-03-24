using UnityEngine;
using System.Collections;

public class TrafficLightBehaviour : MonoBehaviour {

  public Material RedMaterial;
  public Material GreenMaterial;
  private Renderer Renderer;

  public CrossWalkBehaviour CrossWalkReference;

  void Start ()
  {
    Renderer = GetComponent<Renderer>();
    SelectMaterial();
  }

  void Update ()
  {
    SelectMaterial();
  }

  void SelectMaterial()
  {
    if (CrossWalkReference.gameObject.GetComponent<CrossWalkBehaviour>().GetCrossWalkStates == CrossWalkBehaviour.CrossWalkStates.kCrossWalkStates_GreenLight)
      Renderer.material = GreenMaterial;
    else
      Renderer.material = RedMaterial;
  }
}
