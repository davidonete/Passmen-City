using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_PersonBehavior : MonoBehaviour
{
    private GameObject mNextStep;

    private Vector3 end;

    private float mMinDistance;

    private List<Vector3> path = new List<Vector3>();

    private int indexStep;

    private AStarSearch mAStar;

    private float mVelocity;
    
	void Start ()
    {
        mMinDistance = 0.4f;
        mVelocity = 0.02f;
        GetNearestWaypoint();
        //FindNewObjective();
    }
    
	void Update ()
    {
        if (Distance(new Vector3(transform.position.x, transform.position.z), mNextStep) > mMinDistance)
        {
            transform.position += (new Vector3(mNextStep.x, transform.position.y, mNextStep.y) - transform.position).normalized * mVelocity;
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

    private float Distance(Vector3 v1, Vector3 v2)
    {
        float x1 = Mathf.Min(v1.x, v2.x);
        float x2 = Mathf.Max(v1.x, v2.x);
        float y1 = Mathf.Min(v1.y, v2.y);
        float y2 = Mathf.Max(v1.y, v2.y);
        return (x2 - x1) + (y2 - y1);
    }

    private void FindNewObjective()
    {
        Vector3 startPoint = new Vector3((int)transform.position.x, (int)transform.position.z);
        GameObject start = GetNearestWaypoint(startPoint);

        end = new Vector3(Random.Range(0, 9), Random.Range(0, 9));

        mAStar = new AStarSearch(WaypointsExample.grid, start, end);
        
        indexStep = 0;
        mNextStep = GetNearestWaypoint(end);
        while (mNextStep.x != (int)transform.position.x || mNextStep.y != (int)transform.position.z)
        {
            indexStep++;
            path.Add(mNextStep);
            mNextStep = mAStar.cameFrom[mNextStep];
        }
    }

    private GameObject GetNearestWaypoint(Vector3 point)
    {
        return GameObject;
    }
}
