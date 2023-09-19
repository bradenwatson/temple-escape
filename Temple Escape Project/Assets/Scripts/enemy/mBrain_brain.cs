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
    [Header("gameObject components")]
    public mBrain_base initialState;
    public mBrain_base patrolState;
    public mBrain_base attackState;
    public mBrain_base searchPlayerState;
    public mBrain_base searchCollectibleState;
    public mBrain_base searchSoundState;
    mBrain_base currentState;

    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    public Animator animator;

    [Header("links to other gameObject")]
    [SerializeField]
    public GameObject player;

    [Header("leave alone variables")]
    public GameObject currentTarget;
    public bool targetIsPlayer = false;
    Vector3 startingPosition;

    [Header("speed")]
    public float maxSpeed = 10f;
    public float startingSpeed = 3.5f;
    public float increasePerCollectableLost = 1f;    
    public float chasingMultiplier = 1.25f;
    private float currentSpeed;

    [Header("sight")]
    public LayerMask thingsMonsterCanSee;
    public float distanceMonsterCanSee = float.PositiveInfinity;

    [Header("sound")]
    public Vector3 source;
    public float distanceToStopFromSound = 10f;

    // Start is called before the first frame update
    void Start()
    {
        mBrain_base[] states = GetComponents<mBrain_base>();
        print($"states count {states.Count()}");
        for (int i = 0; i < states.Length; i++)
        {
            states[i].isActive = false;
            states[i].AssignBrain(this);
            states[i].AssignStates(patrolState, attackState, searchPlayerState, searchCollectibleState, searchSoundState, animator);
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
        startingPosition = transform.position;

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

    public float GetDistance(Vector3 positionToGetDistanceFor)        // gets distance from one object to another (one of the objects is what its attached to)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, positionToGetDistanceFor, NavMesh.AllAreas, path);

        float distance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return distance;
    }

    public bool SeeIfPlayerIsSeen()
    {       
        if (player != null)
        {
            RaycastHit hit;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, direction, out hit, distanceMonsterCanSee, thingsMonsterCanSee, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.gameObject == player)
                {
                    return true;
                }                
            }
        }
        return false;
    }

    public float GetDistanceToPlayer()
    {
        if (player != null)
        {
            float distance = GetDistance(player.transform.position);
            return distance;
        }
        return 0;
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void MoveToTarget()
    {
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.transform.position);
        }
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
        
        if (isCollectableInfluenced)
        {
            currentSpeed += increasePerCollectableLost;
            agent.speed = currentSpeed;
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

    public bool SeeIfSeachForSound()
    {
        if (currentState != attackState)
        {
            return true;
        }
        return false;
    }

    public void ResetEnemy()
    {
        gameObject.transform.position = startingPosition;
        agent.speed = startingSpeed;
    }
}
