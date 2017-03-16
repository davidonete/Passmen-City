using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowdController : MonoBehaviour
{
    //The time between each crowd iteration check
    public float IterationTime = 1.0f;

    public float SeparationDistance = 0.5f;

    bool mInitialized;
    float mCurrentIterationTime;
    PedestrianBehavior[] mAgents;

	void Start ()
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
	
	void Update ()
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
        for(int i = 0; i < mAgents.Length; i++)
        {
            if (mAgents[i].GetLeader() != null)
            {
                Vector3 finalVelocity = Vector3.zero;
                finalVelocity += Cohesion(mAgents[i]);
                finalVelocity += Separation(mAgents[i]);

                mAgents[i].RigidBody.velocity += finalVelocity;
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
        for(int i = 0; i < neighbours.Count ; i++)
        {
            // Discard self checking
            if(neighbours[i] != agent.gameObject)
            {
                centerOfMass += neighbours[i].transform.position;
                neighbourCount++;
            }
        }

        if(neighbourCount > 0)
        {
            centerOfMass /= neighbourCount;

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
                float distance = (neighbours[i].transform.position - agent.transform.position).magnitude;
                if(distance < SeparationDistance)
                {
                    // Calculate the heading vector between the source entity and its neighbour
                    Vector3 headingVector = agent.transform.position - neighbours[i].transform.position;

                    // Calculate the scale value
                    float scale = headingVector.magnitude / (float)Mathf.Sqrt(SeparationDistance);

                    //The closer we are the more intense the force vector will be
                    forceVector = Vector3.Normalize(headingVector) / scale;
                }
            }
        }
        return forceVector;
    }
}
