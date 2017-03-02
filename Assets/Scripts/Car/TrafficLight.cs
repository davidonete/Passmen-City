using UnityEngine;
using System.Collections;

public class TrafficLight : MonoBehaviour {

  public Material Red;
  public Material Green;
  private Renderer Renderer;

  public float TimebetweenChanges;
  private float SaveTimebetweenChanges;

  // Use this for initialization
  void Start () {
    SaveTimebetweenChanges = TimebetweenChanges;

    Renderer = GetComponent<Renderer>();
    RandomMaterial();
  }
	
	// Update is called once per frame
	void Update () {
    Timer();
	}

  void RandomMaterial()
  {
    int number = Random.Range(0, 2);
   
    if (number == 0)
      Renderer.material = Red;
    else
      Renderer.material = Green;
  }

  void Timer()
  {
    TimebetweenChanges -= Time.deltaTime;
    if (TimebetweenChanges < 0)
    {
      ChangeMaterial();
      TimebetweenChanges = SaveTimebetweenChanges;
    }
  }

  void ChangeMaterial()
  {
    if (Renderer.material.name.Substring(0, Renderer.material.name.Length - 11) == Red.name)
      Renderer.material = Green;
    else
      Renderer.material = Red;
  }
}
