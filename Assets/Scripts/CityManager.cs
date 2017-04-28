using UnityEngine;
using System.Collections;

public class CityManager : MonoBehaviour
{
  //Cars
  public int CarAmount = 0;
  public GameObject Car;

  //Pedestrians
  public int PedestrianAmount = 0;
  public GameObject Pedestrian;
  public GameObject CrowdController;

  // Use this for initialization
  void Start()
  {
    //Generate the city
    GetComponent<CityGenerator>().InitializeCity();

    CreateCars();

    CreatePedestrians();
  }

  // Update is called once per frame
  void Update() { }

  void CreateCars()
  {
    //Generate the AI cars
    GameObject cars = new GameObject();
    cars.name = "Cars";

    for (int i = 0; i < PedestrianAmount; ++i)
    {
      GameObject car = GameObject.Instantiate(Car);
      car.name = "AICar_" + i;
      car.transform.SetParent(cars.transform);
      car.transform.position = AStarSearch.GetRandomWaypoint(WaypointsExample.CarsGraph);
    }
  }

  void CreatePedestrians()
  {
    //Create and initialize the crowd controller
    GameObject crowdController = GameObject.Instantiate(CrowdController);
    crowdController.name = "CrowdController";
    for (int i = 0; i < PedestrianAmount; ++i)
    {
      crowdController.GetComponent<CrowdController>().AddPedestrian();
    }
    crowdController.GetComponent<CrowdController>().Init();
  }
}