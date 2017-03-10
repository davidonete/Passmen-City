using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_PersonBehavior : MonoBehaviour
{
  private Vector3 mNextStep;

  private Vector3 end;

  private List<Vector3> path = new List<Vector3>();

  private int indexStep;

  private AStarSearch mAStar;

  private static float mMinDistance = 0.04f;

  private static float mVelocity = 0.02f;

  void Start()
  {
    FindNewObjective();
  }

  void Update()
  {
    if (Vector3.Distance(transform.position, mNextStep) > mMinDistance)
    {
      //transform.position += (mNextStep - transform.position).normalized * mVelocity;
      transform.position = Vector3.MoveTowards(transform.position, mNextStep, Time.deltaTime * 22.0f);
      Debug.Log(transform.position + " | " + mNextStep);
    }
    else
    {
      if (indexStep > 0)
      {
        indexStep--;
        mNextStep = path[indexStep];
        //Debug.Log("[X: " + mNextStep.x + "] [Y: " + mNextStep.y + "] [Z: " + mNextStep.z + "]");
      }
      else
      {
        transform.position = mNextStep;
        FindNewObjective();
      }
    }
  }

  private void FindNewObjective()
  {
    const float minDistance = 0.1f;
    if (WaypointsExample.CarsGraph.NodesCount() > 0)
    {
      Vector3 start = AStarSearch.GetNearestWaypoint(WaypointsExample.CarsGraph, transform.position);
      end = AStarSearch.GetRandomWaypoint(WaypointsExample.CarsGraph);
      mAStar = new AStarSearch(WaypointsExample.CarsGraph, start, end);

      Debug.Log("Start: " + start + "| End: " + end);
      indexStep = 0;
      mNextStep = end;
      
      while (Vector3.Distance(transform.position, mNextStep) > minDistance)
      {
        indexStep++;
        path.Add(mNextStep);
        Debug.Log("Next " + mNextStep);
        mNextStep = mAStar.cameFrom[mNextStep];
        //Debug.Log(Vector3.Distance(transform.position, mNextStep) + " | " + minDistance);
      }
      Debug.Log("**********************");
    }
  }
}