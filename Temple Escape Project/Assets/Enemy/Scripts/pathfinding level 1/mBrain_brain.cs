using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;
using UnityEngine.AI;
using UnityEngine.Jobs;
using System.Linq;
using static UnityEditor.VersionControl.Asset;

public class mBrain_brain : MonoBehaviour
{
    public mBrain_base initialState;
    public mBrain_base patrolState;
    public mBrain_base attackState;
    public mBrain_base searchPlayerState;
    public mBrain_base searchCollectibleState;
    mBrain_base currentState;

    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    public GameObject player;

    public GameObject currentTarget;
    public bool targetIsPlayer = false;

    public float distanceToAttackPlayer = 10f;

    [Header("speed")]
    public float maxSpeed = 10f;
    public float startingSpeed = 3.5f;
    public float increasePerCollectableLost = 1f;
    public float currentSpeed;
    public float chasingMultiplier = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        mBrain_base[] states = GetComponents<mBrain_base>();
        print($"states count {states.Count()}");
        for (int i = 0; i < states.Length; i++)
        {
            states[i].isActive = false;
            states[i].AssignBrain(this);
            states[i].AssignStates(patrolState, attackState, searchPlayerState, searchCollectibleState);
        }
        if (initialState == null)
        {
            currentState = states[0];
        }
        else
        {
            currentState = initialState;
        }

        currentSpeed = startingSpeed;

        currentState = initialState;
        initialState.OnStateEnter();
    }

    public float GetDistanceToDestination()     // only used for lectures example
    {
        float distance = 0f;
        for (int i = 0; i < agent.path.corners.Length-1; i++)
        {
            distance += Vector3.Distance(agent.path.corners[i], agent.path.corners[i + 1]);
        }
        return distance;
    }

    public float GetDistance(Transform positionToGetDistanceFor)        // gets distance from one object to another (one of the objects is what its attached to)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, positionToGetDistanceFor.position, NavMesh.AllAreas, path);

        float distance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return distance;
    }

    public float GetDistanceToPlayer()
    {
        float distance = GetDistance(player.transform);
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
        if (isPlayer)
        {
            currentTarget = player;
        }
        targetIsPlayer = isPlayer; 
    }

    // Update is called once per frame
    void Update()       // used as the update for all states
    {
        currentState.UpdateState();
    }

    public void RecieveNewState(mBrain_base newState)       // used to say what state the user is on
    {
        currentState = newState;
    }

    public void SetStartingState()
    {
        currentState = initialState;
    }

    public void MonsterSpeed(bool isCollectableInfluenced, bool chasingPlayer, bool lostPlayer, bool reset)         // increases the monster speed based off a multiplier or increasing basespeed
    {
        if (reset)
        {
            agent.speed = startingSpeed;
        }
        
        else if (isCollectableInfluenced)
        {
            agent.speed += increasePerCollectableLost;
            currentSpeed = agent.speed;
        }
        else if (chasingPlayer)
        {
            agent.speed *= chasingMultiplier;
        }
        else if (lostPlayer)
        {
            agent.speed = currentSpeed;
        }
        if (agent.speed > maxSpeed)
        {
            if (!chasingPlayer)
            {
                currentSpeed = maxSpeed;
            }
            agent.speed = maxSpeed;
        }
    }
}
