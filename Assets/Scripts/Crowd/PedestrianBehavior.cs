using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PedestrianBehavior : MonoBehaviour
{
    public bool StartAsLeader = false;
    public float MovementSpeed = 1.0f;

    bool mIsLeader;
    bool mInitialized;
    GameObject mLeader;
    List<GameObject> mNeighbours;

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

        //Begin updating the GameObject
        mInitialized = true;
    }

    void Update()
    {
        if (mInitialized)
        {
            //mRigidBody.velocity = Vector3.Lerp(mRigidBody.velocity, MovementDirection * MovementSpeed, Time.deltaTime);
            RigidBody.MovePosition(transform.position + (Velocity * Time.deltaTime));
        }
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

        else if(GetLeader() != null)
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
            if(agentBehavior.GetLeader() == null)
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
        RigidBody.velocity = Velocity * MovementSpeed;
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

    // Alignment is a behavior that causes a particular agent to line up with agents close by
    Vector3 ComputeAligment()
    {
        return Vector3.zero;
    }

    // Cohesion is a behavior that causes agents to steer towards a "center of mass"
    Vector3 ComputeCohesion(GameObject entity)
    {
        return Vector3.zero;
    }

    Vector3 ComputeSeparation()
    {
        return Vector3.zero;
    }
}
