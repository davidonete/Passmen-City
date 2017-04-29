using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PedestrianBehavior : MonoBehaviour
{
    public enum PedestrianState
    {
        kPedestrianState_Searching,
        kPedestrianState_Walking,
        kPedestrianState_Waiting,
        kPedestrianState_Dead
    }

    bool StartAsLeader = false;
    public float MovementSpeed = 1.0f;

    bool mIsLeader;
    bool mInitialized = false;
    GameObject mLeader;
    List<GameObject> mNeighbours;

    List<Vector3> mPath = new List<Vector3>();
    Vector3 mNextLocation;
    float mMinDistance = 0.04f;

    PedestrianState mState;
    Rigidbody RigidBody;

    public Vector3 Velocity;

    private bool mCollided = false;

    void Start()
    {
        
    }

    // Call this function whenever the game is ready, to start updating the GameObject
    public void Init()
    {
        mState = PedestrianState.kPedestrianState_Searching;
        mLeader = null;
        mNeighbours = new List<GameObject>();
        RigidBody = gameObject.GetComponent<Rigidbody>();
        Velocity = transform.forward;

        ConvertToLeader(StartAsLeader);

        //Begin updating the GameObject
        mInitialized = true;
    }

    void Update()
    {
        if (mInitialized)
        {
            switch (mState)
            {
                case PedestrianState.kPedestrianState_Searching:
                    Searching();
                    break;

                case PedestrianState.kPedestrianState_Walking:
                    Walking();
                    break;

                case PedestrianState.kPedestrianState_Waiting:
                    Waiting();
                    break;

                case PedestrianState.kPedestrianState_Dead:
                    break;

                default:
                    break;
            }
        }
    }

    void Searching()
    {
        //Start pathfinding (get nearest point and get a random point)
        GetNewPath();
        if(GetNextLocationStep())
            mState = PedestrianState.kPedestrianState_Walking;
    }

    void Walking()
    {
        if (CheckCrosswalk())
            mState = PedestrianState.kPedestrianState_Waiting;
        else if(CheckCar())
            mState = PedestrianState.kPedestrianState_Waiting;
        else
        {
            //If the agent is affected by flocking
            if (GetLeader())
            {
                transform.forward = Velocity.normalized;
                RigidBody.MovePosition(transform.position + (Velocity * Time.deltaTime * MovementSpeed));
            
                //TO DO: Check if the distance has increased since last update
            }
            else
                UpdatePathfindingMovement();
        }
    }

    bool CheckCrosswalk()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.0f))
        {
            if (hit.collider.gameObject.tag == "CrossWalk")
            {
                CrossWalkBehaviour crosswalk = hit.collider.gameObject.GetComponent<CrossWalkBehaviour>();
                if (crosswalk.GetCrossWalkStates == CrossWalkBehaviour.CrossWalkStates.kCrossWalkStates_RedLight)
                {
                    crosswalk.SetIsPedestrianWaiting(true);
                    return true;
                }
            }
        }
        return false;
    }

    bool CheckCar()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.0f))
        {
            if (hit.collider.gameObject.tag == "Car")
                return true;
        }

        return false;
    }

    void Waiting()
    {
        if (!CheckCrosswalk())
            mState = PedestrianState.kPedestrianState_Walking;
        else if (!CheckCar())
            mState = PedestrianState.kPedestrianState_Walking;
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
        PedestrianBehavior agentBehavior = agent.GetComponent<PedestrianBehavior>();
        if (agentBehavior.GetLeader() == null)
        {
            if (IsLeader() && !mNeighbours.Contains(agent))
            {
                agentBehavior.SetLeader(gameObject);
                mNeighbours.Add(agent);
            }

            //Autogenerate leader of the flocking group
            else if (!IsLeader() && GetLeader() == null)
            {
                ConvertToLeader(true);
                agentBehavior.ConvertToLeader(false);
                agentBehavior.SetLeader(gameObject);
                mNeighbours.Add(agent);
            }
        }
    }

    public void RemoveNeighbour(GameObject agent)
    {
        PedestrianBehavior agentBehavior = agent.GetComponent<PedestrianBehavior>();
        if (IsLeader() && mNeighbours.Contains(agent))
        {
            agentBehavior.SetLeader(null);
            mNeighbours.Remove(agent);

            //If the leader has no neighbours
            if (mNeighbours.Count <= 0)
                ConvertToLeader(false);
        }
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
            mNextLocation.y = 0.0f;
            mPath.RemoveAt(mPath.Count - 1);

            return true;
        }
        return false;
    }

    void UpdatePathfindingMovement()
    {
        if (Vector3.Distance(transform.position, mNextLocation) > mMinDistance)
        {
            transform.forward = (mNextLocation - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, mNextLocation, Time.deltaTime * MovementSpeed);
        }
        else
        {
            transform.position = mNextLocation;
            if (!GetNextLocationStep())
                mState = PedestrianState.kPedestrianState_Searching;
        }
    }

    public PedestrianState GetPedestrianState
    {
        get { return mState; }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Car" && !mCollided)
        {
            Debug.Log("AUUUUUUU");
            mCollided = true;
            mState = PedestrianState.kPedestrianState_Dead;
            GetComponent<RagdollController>().EnableRagdoll();
            GetComponent<Animator>().enabled = false;
        }
    }
}
