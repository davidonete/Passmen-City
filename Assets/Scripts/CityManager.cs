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
    //Generate the AI pedestrians
    GameObject pedestrians = new GameObject();
    pedestrians.name = "Pedestrians";

    for (int i = 0; i < PedestrianAmount; ++i)
    {
      GameObject pedestrian = GameObject.Instantiate(Pedestrian);
      pedestrian.name = "AIPedestrian_" + i;
      pedestrian.transform.SetParent(pedestrians.transform);
      Vector3 position = AStarSearch.GetRandomWaypoint(WaypointsExample.PedestriansGraph);
      position.y = 1.0f;
      pedestrian.transform.position = position;
    }

    //Create and initialize the crowd controller
    GameObject crowdController = GameObject.Instantiate(CrowdController);
    crowdController.name = "CrowdController";
    crowdController.GetComponent<CrowdController>().Init();
  }
}