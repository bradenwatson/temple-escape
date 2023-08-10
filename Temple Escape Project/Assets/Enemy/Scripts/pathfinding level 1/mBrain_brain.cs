using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;
using UnityEngine.AI;
using UnityEngine.Jobs;
using System.Linq;

public class mBrain_brain : MonoBehaviour
{
    public mBrain_base initialState;
    mBrain_base currentState;

    [SerializeField]
    NavMeshAgent agent;

    public GameObject currentTarget;
    public bool targetIsPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        mBrain_base[] states = GetComponents<mBrain_base>();
        print($"states count {states.Count()}");
        for (int i = 0; i < states.Length; i++)
        {
            states[i].isActive = false;
            states[i].AssignBrain(this);
        }
        if (initialState == null)
        {
            currentState = states[0];
        }
        else
        {
            currentState = initialState;
        }

        currentState = initialState;
        initialState.OnStateEnter();
    }

    public float GetDistanceToDestination()
    {
        float distance = 0f;
        for (int i = 0; i < agent.path.corners.Length-1; i++)
        {
            distance += Vector3.Distance(agent.path.corners[i], agent.path.corners[i + 1]);
        }
        return distance;
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void MoveToTarget()
    {
        agent.SetDestination(currentTarget.transform.position);
    }

    public void AssignTarget(GameObject target, bool isPlayer)
    {
        currentTarget = target;
        targetIsPlayer = isPlayer; 
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }
}
