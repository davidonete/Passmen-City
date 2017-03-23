using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowdController : MonoBehaviour
{
  //The time between each crowd iteration check
  public float IterationTime = 1.0f;

  public float SeparationDistance = 0.5f;
  public float SeparationWeight = 1.0f;
  public float CohesionWeight = 1.0f;
  public float AlignmentWeight = 1.0f;

  bool mInitialized;
  float mCurrentIterationTime;
  PedestrianBehavior[] mAgents;

  Vector3 centerOfMas;

  void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawSphere(centerOfMas, 1.0f);
  }

  void Start()
  {
    mInitialized = false;

    //Temporal initialize (delete this when generating procedural city)
    Init();
  }

  // Call this function whenever the game is ready, to start updating the GameObject
  public void Init()
  {
    //Get all agents of the map, save its reference and initialize them
    mAgents = FindObjectsOfType<PedestrianBehavior>();
    for (uint i = 0; i < mAgents.Length; i++)
      mAgents[i].Init();

    //Begin updating the GameObject
    mInitialized = true;
  }

  void Update()
  {
    if (mInitialized)
    {
      if (mCurrentIterationTime >= IterationTime)
      {
        mCurrentIterationTime = 0.0f;

        //Check crowd iteration
        ComputeFlocking();
      }
      else
        mCurrentIterationTime += Time.deltaTime;
    }
  }

  void ComputeFlocking()
  {
    for (int i = 0; i < mAgents.Length; i++)
    {
      if (mAgents[i].GetLeader() != null)
      {
        mAgents[i].Velocity += Alignment(mAgents[i]) * AlignmentWeight;
        mAgents[i].Velocity += Cohesion(mAgents[i]) * CohesionWeight;
        mAgents[i].Velocity += Separation(mAgents[i]) * SeparationWeight;
      }
    }
  }

  // Cohesion is a behavior that causes agents to steer towards a "center of mass"
  Vector3 Cohesion(PedestrianBehavior agent)
  {
    Vector3 forceVector = Vector3.zero;
    Vector3 centerOfMass = Vector3.zero;
    uint neighbourCount = 0;

    List<GameObject> neighbours = agent.GetNeighbours();
    for (int i = 0; i < neighbours.Count; i++)
    {
      // Discard self checking
      if (neighbours[i] != agent.gameObject)
      {
        centerOfMass += neighbours[i].transform.position;
        neighbourCount++;
      }
    }

    if (neighbourCount > 0)
    {
      centerOfMass /= neighbourCount;

      centerOfMas = centerOfMass;

      forceVector = centerOfMass - agent.gameObject.transform.position;
      forceVector.Normalize();
    }
    return forceVector;
  }

  Vector3 Separation(PedestrianBehavior agent)
  {
    Vector3 forceVector = Vector3.zero;

    List<GameObject> neighbours = agent.GetNeighbours();
    for (int i = 0; i < neighbours.Count; i++)
    {
      // Discard self checking
      if (neighbours[i] != agent.gameObject)
      {
        // Distance squared
        float distance = Mathf.Pow(Vector3.Distance(neighbours[i].transform.position, agent.transform.position), 2.0f);
        if (distance < SeparationDistance)
        {
          // Calculate the heading vector between the source entity and its neighbour
          Vector3 headingVector = agent.transform.position - neighbours[i].transform.position;

          // Calculate the scale value
          float scale = headingVector.magnitude / (float)Mathf.Sqrt(SeparationDistance);
          //scale = 1.0f - (headingVector.magnitude / SeparationDistance);
          //scale = Mathf.Max(scale, 0.001f);

          //The closer we are the more intense the force vector will be
          forceVector = Vector3.Normalize(headingVector) / scale;
        }
      }
    }
    //Debug.Log("Forward: " + agent.transform.forward + " ForceVector: " + forceVector + " Velocity: " + agent.Velocity);
    return forceVector;
  }

  Vector3 Alignment(PedestrianBehavior agent)
  {
    Vector3 forceVector = Vector3.zero;
    int neighboursCount = 0;

    List<GameObject> neighbours = agent.GetNeighbours();
    for (int i = 0; i < neighbours.Count; i++)
    {
      // Discard self checking
      if (neighbours[i] != agent.gameObject)
      { 
        forceVector += neighbours[i].GetComponent<PedestrianBehavior>().Velocity;
        neighboursCount++;
      }
    }

    if(neighboursCount > 0)
    {
      forceVector /= neighboursCount;
      forceVector.Normalize();
    }

    return forceVector;
  }
}
