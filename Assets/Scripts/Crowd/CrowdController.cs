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

    bool mInitialized = false;
    float mCurrentIterationTime;
    List<PedestrianBehavior> mAgents = new List<PedestrianBehavior>();
    public GameObject PedestrianPrefab;
    private GameObject mPedestriansParent;
    NN.NeuralNetwork mNNController = null;

    Vector3 centerOfMas;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(centerOfMas, 1.0f);
    }

    // Call this function whenever the game is ready, to start updating the GameObject
    public void Init()
    {
        mInitialized = true;
    }

    void Update()
    {
        //Debug.Log("Pedestrians:" + mAgents.Count);
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
        for (int i = 0; i < mAgents.Count; i++)
        {
            if (mAgents[i].GetLeader() != null)
            {
                mAgents[i].Velocity += Alignment(mAgents[i]) * AlignmentWeight;
                mAgents[i].Velocity += Cohesion(mAgents[i]) * CohesionWeight;
                mAgents[i].Velocity += Separation(mAgents[i]) * SeparationWeight;
                mAgents[i].Velocity.Normalize();
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

                    //The closer we are the more intense the force vector will be
                    forceVector = Vector3.Normalize(headingVector) / scale;
                }
            }
        }
        return forceVector;
    }

    // Alignment is a behavior that causes a particular agent to line up with agents close by
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

        if (neighboursCount > 0)
        {
            forceVector /= neighboursCount;
            forceVector.Normalize();
        }

        return forceVector;
    }

    /// <summary>
    /// Adds a pedestrian to the simulation
    /// </summary>
    public void AddPedestrian()
    {
        // Yes, it is a hack. Deal with it
        if(!mPedestriansParent)
        {
            mPedestriansParent = new GameObject();
            mPedestriansParent.name = "Pedestrians";
        }

        GameObject pedestrian = GameObject.Instantiate(PedestrianPrefab);
        pedestrian.name = "AIPedestrian_" + mAgents.Count;
        pedestrian.transform.SetParent(mPedestriansParent.transform);
        Vector3 position = AStarSearch.GetRandomWaypoint(WaypointsExample.PedestriansGraph);
        position.y = 0.0f;
        pedestrian.transform.position = position;

        // Init the pedestrian behaviour
        pedestrian.GetComponent<PedestrianBehavior>().Init();
        pedestrian.GetComponent<NN.NeuralNetwork>().Init();

        if(mNNController == null)
        {
            mNNController = GameObject.FindGameObjectWithTag("Neural Network Controller").GetComponent<NN.NeuralNetwork>();
            mNNController.Init();
        }
        
        pedestrian.GetComponent<NN.NeuralNetwork>().SetConnectionWeights(mNNController.GetConnectionWeights());
        mAgents.Add(pedestrian.GetComponent<PedestrianBehavior>());
    }

    /// <summary>
    /// Removes the provided pedestrian from the simulation
    /// </summary>
    /// <param name="pedestrian"> The pedestrian to be removed </param>
    public void RemovePedestrian(PedestrianBehavior pedestrian)
    {
        mAgents.Remove(pedestrian);
        AddPedestrian();
    }
}

