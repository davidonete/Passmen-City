using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PedestrianBehavior : MonoBehaviour
{
    bool StartAsLeader = false;
    public float MovementSpeed = 1.0f;

    bool mIsLeader;
    bool mInitialized;
    GameObject mLeader;
    List<GameObject> mNeighbours;

    List<Vector3> mPath = new List<Vector3>();
    Vector3 mNextLocation;
    float mMinDistance = 0.04f;

    Rigidbody RigidBody;
    public Vector3 Velocity;

    void Start()
    {
        mInitialized = false;
    }

    // Call this function whenever the game is ready, to start updating the GameObject
    public void Init()
    {
        mLeader = null;
        mNeighbours = new List<GameObject>();
        RigidBody = gameObject.GetComponent<Rigidbody>();
        Velocity = transform.forward;

        ConvertToLeader(StartAsLeader);

        //Start pathfinding (get nearest point and get a random point)
        GetNewPath();
        GetNextLocationStep();

        //Begin updating the GameObject
        mInitialized = true;
    }

    void Update()
    {
        if (mInitialized)
        {
            //If the agent is affected by flocking
            if (GetLeader())
                RigidBody.MovePosition(transform.position + (Velocity * Time.deltaTime * MovementSpeed));
            else
                UpdatePathfindingMovement();
        }
        if (mIsLeader)
            MoveToMouseClick();
    }

    bool IsLeader() { return mIsLeader; }

    public GameObject GetLeader() { return mLeader; }

    public void SetLeader(GameObject leader) { mLeader = leader; }

    void ConvertToLeader(bool leader)
    {
        mLeader = null;
        mIsLeader = leader;
        mNeighbours.Clear();

        //Add yourself to the flocking group (for computing)
        if (mIsLeader)
            mNeighbours.Add(gameObject);
    }

    public List<GameObject> GetNeighbours()
    {
        if (IsLeader())
            return mNeighbours;

        else if (GetLeader() != null)
        {
            PedestrianBehavior leader = GetLeader().GetComponent<PedestrianBehavior>();
            return leader.GetNeighbours();
        }

        return null;
    }

    public void AddNeighbour(GameObject agent)
    {
        if (IsLeader() && !mNeighbours.Contains(agent))
        {
            PedestrianBehavior agentBehavior = agent.GetComponent<PedestrianBehavior>();
            if (agentBehavior.GetLeader() == null)
            {
                agentBehavior.SetLeader(gameObject);
                mNeighbours.Add(agent);
            }
        }
        // TO-DO: Autogenerate leader of the flocking group
    }

    public void RemoveNeighbour(GameObject agent)
    {
        PedestrianBehavior agentBehavior = agent.GetComponent<PedestrianBehavior>();
        if (IsLeader() && mNeighbours.Contains(agent))
        {
            agentBehavior.SetLeader(null);
            mNeighbours.Remove(agent);
        }
        // TO-DO: Autogenerate leader of the flocking group
    }

    void MoveToMouseClick()
    {
        //RigidBody.velocity = Velocity * MovementSpeed;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Velocity = hit.point - gameObject.transform.position;
                Velocity.Normalize();
            }
        }
    }

    void GetNewPath()
    {
        Vector3 start = AStarSearch.GetNearestWaypoint(WaypointsExample.PedestriansGraph, transform.position);
        Vector3 end = AStarSearch.GetRandomWaypoint(WaypointsExample.PedestriansGraph);
        mPath = AStarSearch.FindNewObjective(WaypointsExample.PedestriansGraph, start, end);
    }

    //Return whenever there is no more seps
    bool GetNextLocationStep()
    {
        if (mPath.Count > 0)
        {
            mNextLocation = mPath[mPath.Count - 1];
            mPath.RemoveAt(mPath.Count - 1);

            return true;
        }
        return false;
    }

    void UpdatePathfindingMovement()
    {
        if (Vector3.Distance(transform.position, mNextLocation) > mMinDistance)
            transform.position = Vector3.MoveTowards(transform.position, mNextLocation, Time.deltaTime * MovementSpeed);
        else
        {
            transform.position = mNextLocation;
            if (!GetNextLocationStep())
                GetNewPath();
        }
    }
}
