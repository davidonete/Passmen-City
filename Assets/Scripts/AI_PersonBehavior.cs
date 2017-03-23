using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_PersonBehavior : MonoBehaviour
{
	private const float mMinDistance = 0.04f;

	private const float mVelocity = 2.0f;

	private Vector3 mNextStep;

	private List<Vector3> mPath = new List<Vector3>();

	void Start()
	{
		Vector3 start = AStarSearch.GetNearestWaypoint(WaypointsExample.CarsGraph, transform.position);
		Vector3 end = AStarSearch.GetRandomWaypoint(WaypointsExample.CarsGraph);
		mPath = AStarSearch.FindNewObjective(WaypointsExample.CarsGraph, start, end);
		if (mPath.Count > 0) mNextStep = mPath[mPath.Count - 1];
	}

	void Update()
	{
		if (Vector3.Distance(transform.position, mNextStep) > mMinDistance)
		{
			transform.position = Vector3.MoveTowards(transform.position, mNextStep, Time.deltaTime * mVelocity);
		}
		else
		{
			if (mPath.Count > 0) mPath.RemoveAt(mPath.Count - 1);
			if (mPath.Count > 0)
			{
				mNextStep = mPath[mPath.Count - 1];
			}
			else
			{
				transform.position = mNextStep;

				Vector3 start = AStarSearch.GetNearestWaypoint(WaypointsExample.CarsGraph, transform.position);
				Vector3 end = AStarSearch.GetRandomWaypoint(WaypointsExample.CarsGraph);
				mPath = AStarSearch.FindNewObjective(WaypointsExample.CarsGraph, start, end);
        if (mPath.Count > 0) mNextStep = mPath[mPath.Count - 1];
			}
		}
	}
}
