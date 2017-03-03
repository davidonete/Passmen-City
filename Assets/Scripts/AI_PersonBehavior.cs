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

  private static float mMinDistance = 0.4f;

  private static float mVelocity = 0.02f;

  void Start()
  {
    FindNewObjective();
  }

  void Update()
  {
    if (Vector3.Distance(transform.position, mNextStep) > mMinDistance)
    {
      transform.position += (mNextStep - transform.position).normalized * mVelocity;
      //Debug.Log("[]");
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
        FindNewObjective();
      }
    }
  }

  private void FindNewObjective()
  {
    if (WaypointsExample.grid.NodesCount() > 0)
    {
      Vector3 start = AStarSearch.GetNearestWaypoint(WaypointsExample.grid, transform.position);
      end = AStarSearch.GetRandomWaypoint(WaypointsExample.grid);
      mAStar = new AStarSearch(WaypointsExample.grid, start, end);

      Debug.Log("Start: " + start + "| End: " + end);
      indexStep = 0;
      mNextStep = end;
      while (Vector3.Distance(transform.position, mNextStep) > mMinDistance)
      {
        indexStep++;
        path.Add(mNextStep);
        Debug.Log("Next " + mNextStep);
        mNextStep = mAStar.cameFrom[mNextStep];
        Debug.Log("Next " + mNextStep);
      }
      Debug.Log(" ");
    }
  }
}