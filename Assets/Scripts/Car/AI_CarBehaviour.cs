using UnityEngine;
using System.Collections;

public class AICarBehaviour : MonoBehaviour {

    private enum CarStates
    {
        kCarState_Searching,
        kCarState_Driving,
        kCarState_Waiting
    }

    private struct CarConditions
    {
        // Searching State
        private bool IsSearching;
        // Driving State
        private bool IsDriving;
        // Waiting State
        private bool IsWaiting;
    }

    private CarStates State;


    void Start()
    {
        State = CarStates.kCarState_Searching;
    }

    void Update()
    {
        StateMachine();
    }

    void StateMachine()
    {
        switch (State)
        {
            case CarStates.kCarState_Searching:
                Searching();
                break;

            case CarStates.kCarState_Driving:

                break;

            case CarStates.kCarState_Waiting:

                break;
        }
    }

    // Searching for a new direction
    void Searching()
    {

    }
}
