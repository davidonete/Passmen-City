using UnityEngine;
using System.Collections;

public class CrowdController : MonoBehaviour
{
    //The time between each crowd iteration check
    public float IterationTime;

    float mCurrentIterationTime;
    AI_PersonBehavior[] mAgents;

	// Use this for initialization
	void Start ()
    {
        Init();
	}

    public void Init()
    {
        //Get all agents of the map and save its reference
        mAgents = FindObjectsOfType<AI_PersonBehavior>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(mCurrentIterationTime >= IterationTime)
        {
            mCurrentIterationTime = 0.0f;
            //Check crowd iteration
            //...
        }
        else
            mCurrentIterationTime += Time.deltaTime;
	}
}
