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
    if (Distance(transform.position, mNextStep) > mMinDistance)
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

      indexStep = 0;
      mNextStep = AStarSearch.GetNearestWaypoint(WaypointsExample.grid, end);
      while (mNextStep.x != (int)transform.position.x || mNextStep.y != (int)transform.position.z)
      {
        indexStep++;
        path.Add(mNextStep);
        mNextStep = mAStar.cameFrom[mNextStep];
      }
    }
  }

  private static float Distance(Vector3 v1, Vector3 v2)
  {
    float x1 = Mathf.Min(v1.x, v2.x);
    float x2 = Mathf.Max(v1.x, v2.x);
    float y1 = Mathf.Min(v1.y, v2.y);
    float y2 = Mathf.Max(v1.y, v2.y);
    return (x2 - x1) + (y2 - y1);
  }
}
