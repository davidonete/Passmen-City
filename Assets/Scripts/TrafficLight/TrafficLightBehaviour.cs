using UnityEngine;
using System.Collections;

public class TrafficLightBehaviour : MonoBehaviour
{
    private Renderer mRenderer;
    private Material mMaterial;
    private bool mIsOpen;
    public CrossWalkBehaviour CrossWalkRef;

    void Start()
    {
        mRenderer = GetComponent<Renderer>();
        mMaterial = mRenderer.material;
        mIsOpen = true;
    }

    void Update()
    {
        // Green light
        if(CrossWalkRef.GetCrossWalkStates == CrossWalkBehaviour.CrossWalkStates.kCrossWalkStates_GreenLight)
        {
            if(!mIsOpen)
            {
                mMaterial.SetColor("_Color", Color.green);
                mMaterial.SetColor("_EmissionColor", Color.green);
                mIsOpen = true;
            }
        }
        // Red light
        else
        {
            if(mIsOpen)
            {
                mMaterial.SetColor("_Color", Color.red);
                mMaterial.SetColor("_EmissionColor", Color.red);
                mIsOpen = false;
            }
        }
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
