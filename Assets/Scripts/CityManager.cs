using UnityEngine;
using System.Collections;

public class CityManager : MonoBehaviour
{
  //Pedestrians
  public int PedestrianAmount = 0;
  public GameObject Pedestrian;

  //Cars
  public int CarAmount = 0;
  public GameObject Car;

  // Use this for initialization
  void Start()
  {
    //Generate the city
    GetComponent<CityGenerator>().InitializeCity();

    CreatePedestrians();

    CreateCars();
  }

  // Update is called once per frame
  void Update() { }

  void CreatePedestrians()
  {
    //Generate the AI pedestrians
    GameObject pedestrians = new GameObject();
    pedestrians.name = "Pedestrians";

    for (int i = 0; i < PedestrianAmount; ++i)
    {
      GameObject pedestrian = GameObject.Instantiate(Pedestrian);
      pedestrian.name = "AIPedestrian_" + i;
      pedestrian.transform.position = new Vector3(i, 0.0f, 0.0f);
      pedestrian.transform.SetParent(pedestrians.transform);
    }
  }

  void CreateCars()
  {
    //Generate the AI cars
    GameObject cars = new GameObject();
    cars.name = "Cars";

    for (int i = 0; i < PedestrianAmount; ++i)
    {
      GameObject car = GameObject.Instantiate(Car);
      car.name = "AICar_" + i;
      car.transform.position = new Vector3(i, 0.0f, 1);
      car.transform.SetParent(cars.transform);
    }
  }
}