using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Jobs;
using System.Linq;

public class mBrain_brain : MonoBehaviour
{
    [Header("gameObject components")]
    [Tooltip("the state the GameObject starts in at launch (usually just make it same as patrol state)")]
    public mBrain_base initialState;
    [Tooltip("the state the GameObject uses when it is patroling")]
    public mBrain_base patrolState;
    [Tooltip("the state you wish to use when the GameObject is attacking another GameObject")]
    public mBrain_base attackState;
    [Tooltip("the state you wish to use when the GameObject is trying to find another GameObject (player)")]
    public mBrain_base searchPlayerState;
    [Tooltip("the state you wish to use when the GameObject is looking for another GameObject (collectables)")]
    public mBrain_base searchCollectibleState;
    [Tooltip("the state you wish to use when the GameObject is looking for sound (caused when method called)")]
    public mBrain_base searchSoundState;
    mBrain_base currentState;

    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    [Tooltip("the animator you wish the enemy to use")]
    public Animator animator;

    [Header("links to other gameObject")]
    [SerializeField]
    [Tooltip("the reference to the player GameObject")]
    public GameObject player;

    [Header("leave alone variables")]
    [Tooltip("the GameObject this GameObject is focused on (used by other states - leave alone)")]
    public GameObject currentTarget;
    [Tooltip("says if the target is the player (dont trust not used - leave alone)")]
    public bool targetIsPlayer = false;
    Vector3 startingPosition;

    [Header("speed")]
    [Tooltip("the max speed you wish this GameObject to move at when using agent set destination")]
    public float maxSpeed = 10f;
    [Tooltip("the speed the GameObject moves at on launch when using agent set destination")]
    public float startingSpeed = 3.5f;
    [Tooltip("the amount of points the GameObject speeds up (+=) when using agent set destination")]
    public float increasePerCollectableLost = 1f;
    [Tooltip("the multiplier (*=) used when the enemy has found the player and is chasing that GameObject")]
    public float chasingMultiplier = 1.25f;
    float currentSpeed;

    [Header("sight")]
    [Tooltip("everything the monster can see (example: if there is a wall on a scene layer and the scene layer is not selected it will act like glass, for us, and be able to see straight through it)")]
    public LayerMask thingsMonsterCanSee;
    [Tooltip("the distance the raycast will be sent out")]
    public float distanceMonsterCanSee = float.PositiveInfinity;

    [Header("sound")]
    [Tooltip("the source of the sound (gets set through a method call which gets called by other GameObjects - nothing uses it yet - debuging only)")]
    public Vector3 source;
    [Tooltip("the distance the monster will stop from the source of the sound")]
    public float distanceToStopFromSound = 10f;

    [Header("audio source")]
    [Tooltip("the audio source to be used for footsteps")]
    public AudioSource footSteps;
    [Tooltip("the audio source to be used for attacking and growling")]
    public AudioSource growlingAndAttack;

    /// <summary>
    /// assigns all the inherited variables to each state
    /// </summary>
    void Start()
    {
        mBrain_base[] states = GetComponents<mBrain_base>();
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
    
    /// <summary>
    /// gets the distance to GameObject - it isnt a straight line, it accounts for corners. Will return 0 if it is impossible for the GameObject to get to the destination (need to fix)
    /// </summary>
    /// <param name="positionToGetDistanceFor">the position you wish to see how far to (example: if you want to see the distance to player from the enemy, enter the players position as the argument)</param>
    /// <returns>the distance from this GameObject to another. Will return 0 if impossible to reach destination</returns>
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

    /// <summary>
    /// sees if this GameObject can see the player. Silent error for if the distance to the player is 0
    /// </summary>
    /// <returns></returns>
    public bool SeeIfPlayerIsSeen()
    {        
        if (player != null)
        {
            if (GetDistance(player.transform.position) == 0)
            {
                return false;
            }

            RaycastHit hit;
            Vector3 direction = (player.transform.position - transform.position);
            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, out hit, direction.magnitude, thingsMonsterCanSee, QueryTriggerInteraction.Collide))
            {
                //print(hit.collider.gameObject.layer);
                print(hit.collider.gameObject.name);
                if (hit.collider.gameObject == player)
                {
                    print("hit player");
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// makes the GameObject move to the destination
    /// </summary>
    /// <param name="destination">the place you wish this GameObject to end up</param>
    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    /// <summary>
    /// makes the GameObject move to current target (use AssignTarget to assign the target)
    /// </summary>
    public void MoveToTarget()
    {
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.transform.position);
        }
    }

    /// <summary>
    /// assigns the target the GameObject will move to (use MoveToTarget to move to the target)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="isPlayer"></param>
    public void AssignTarget(GameObject target, bool isPlayer)
    {
        currentTarget = target;
        if (isPlayer)
        {
            currentTarget = player;
        }
        targetIsPlayer = isPlayer; 
    }

    /// <summary>
    /// runs the update state which gets overwriten for each state call
    /// </summary>
    void Update()
    {
        currentState.UpdateState();
    }

    /// <summary>
    /// changes the current state to the new state
    /// </summary>
    /// <param name="newState">the state you wish to replace current state (will need to use transition state to change state)</param>
    public void RecieveNewState(mBrain_base newState)
    {
        currentState = newState;
    }
    
    /// <summary>
    /// makes the current state the initial state
    /// </summary>
    public void SetStartingState()
    {
        currentState = initialState;
    }

    /// <summary>
    /// uses to alter the GameObjects speed
    /// </summary>
    /// <param name="isCollectableInfluenced">increases the current speed</param>
    /// <param name="chasingPlayer">applies a multiplier to the GameObjecs speed</param>
    /// <param name="lostPlayer">makes the agent speed equal to current speed (takes away chasing player multiplier)</param>
    /// <param name="reset">resets the current speed of the monster back to how it started</param>
    public void MonsterSpeed(bool isCollectableInfluenced=false, bool chasingPlayer=false, bool lostPlayer=false, bool reset=false)         // increases the monster speed based off a multiplier or increasing basespeed
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
            agent.speed = currentSpeed * chasingMultiplier;
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

    /// <summary>
    /// sees if the GameObjects current state is attack state
    /// </summary>
    /// <returns>true if not attack state and false if it is</returns>
    public bool SeeIfSeachForSound()
    {
        if (currentState != attackState)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// resets the speed and position of the enemy back to how it started
    /// </summary>
    public void ResetEnemy()
    {
        gameObject.transform.position = startingPosition;
        MonsterSpeed(reset:true);
    }

    /// <summary>
    /// players the foot steps of the GameObject
    /// </summary>
    public void PlayFootSteps()
    {
        PlaySound.PlaySoundOnRepeat("Enemy_Footstep", footSteps);
    }

    /// <summary>
    /// stops the foot steps of the enemy
    /// </summary>
    public void StopFootSteps()
    {
        PlaySound.StopSound(footSteps);
    }

    /// <summary>
    /// plays the growling sound of the enemy
    /// </summary>
    public void PlayGrowling()
    {
        PlaySound.PlaySoundOnce("Enemy_Growl", growlingAndAttack);
    }

    /// <summary>
    /// plays the sound of attacking the enemy
    /// </summary>
    public void PlayerAttackingSound()
    {
        PlaySound.PlaySoundOnce("Staff_Knock", growlingAndAttack);
    }
}
