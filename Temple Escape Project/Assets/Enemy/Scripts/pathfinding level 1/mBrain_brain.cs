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

    public float GetDistanceToDestination()
    {
        float distance = 0f;
        for (int i = 0; i < agent.path.corners.Length-1; i++)
        {
            distance += Vector3.Distance(agent.path.corners[i], agent.path.corners[i + 1]);
        }
        return distance;
    }

    public float GetDistance(Transform positionToGetDistanceFor)
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
    void Update()
    {
        currentState.UpdateState();
    }

    public void RecieveNewState(mBrain_base newState)
    {
        currentState = newState;
    }

    public void SetStartingState()
    {
        currentState = initialState;
    }

    public void MonsterSpeed(bool isCollectableInfluenced, bool chasingPlayer, bool lostPlayer)
    {
        if (isCollectableInfluenced)
        {
            agent.speed += increasePerCollectableLost;
            currentSpeed = agent.speed;
        }
        if (chasingPlayer)
        {
            agent.speed *= chasingMultiplier;
        }
        if (lostPlayer)
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
