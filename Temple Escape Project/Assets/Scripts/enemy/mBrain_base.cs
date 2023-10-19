using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class mBrain_base : MonoBehaviour
{
    public bool isActive = false;
    internal mBrain_brain brain;
    internal mBrain_base patrolState;
    internal mBrain_base attackState;
    internal mBrain_base searchPlayerState;
    internal mBrain_base searchCollectibleState;
    internal mBrain_base searchSoundState;
    internal PlaySound soundController;
    internal Animator animator;

    public void OnStateEnter()
    {
        isActive = true;
        OnStateEnterArgs();
    }

    /// <summary>
    /// assigns the brain to each state
    /// </summary>
    /// <param name="brain">is the mBrain_brain class instance used to link each class together</param>
    public void AssignBrain(mBrain_brain brain)
    {
        this.brain = brain;
    }

    /// <summary>
    /// meant to be overriten so when each state gets activated you can set something to happen (like patrol)
    /// </summary>
    internal virtual void OnStateEnterArgs()
    {

    }

    public abstract void UpdateState();

    /// <summary>
    /// used to transition to next state (the states to use should be already avaible through inheritance)
    /// </summary>
    /// <param name="nextState"></param>
    public void TransitionToNextState(mBrain_base nextState)
    {
        OnStateExit();
        brain.RecieveNewState(nextState);
        nextState.OnStateEnter();
        
    }

    /// <summary>
    /// a function to control what happens when you stop a state
    /// </summary>
    void OnStateExit() 
    { 
        isActive = false;
        OnStateExitArgs();
    }

    /// <summary>
    /// meant to be overriten, controls what happens when you exit a state
    /// </summary>
    internal virtual void OnStateExitArgs()
    {

    }

    /// <summary>
    /// where you make it so each state has access to other states so you can transition between states. To Create more stats you will need to create a new argument for that state, enter it into the brain and
    /// set it the same way the others are set
    /// </summary>
    /// <param name="patrolState">the patroling state you wish the GameObject to use when it needs to patrol</param>
    /// <param name="attackState">the attacking state you wish the GameObject to use when it needs to attack something</param>
    /// <param name="searchForPlayer">the state you wish the enemy to enter when it needs to search for player (usually called after lost sight)</param>
    /// <param name="checkCollectableState">the state that is called when you want the monster to look for collectable (called after couldn't find player</param>
    /// <param name="searchSoundState">the state you wish to be called when a sound is made and you want the GameObject to go towards it</param>
    /// <param name="animator">the animator that controls all the GameObject animations</param>
    public void AssignStates(mBrain_base patrolState, mBrain_base attackState, mBrain_base searchForPlayer, mBrain_base checkCollectableState, mBrain_base searchSoundState, Animator animator)
    {
        this.patrolState = patrolState;
        this.attackState = attackState;
        this.searchPlayerState = searchForPlayer;
        this.searchCollectibleState = checkCollectableState;
        this.searchSoundState = searchSoundState;
        this.animator = animator;
    }

    /// <summary>
    /// method to be called by other GameObjects. will make the GameObject go towards the vector3 (argument)
    /// </summary>
    /// <param name="sourceOfSound">the source of the sound where the enemy will go to</param>
    public void SearchSound(Vector3 sourceOfSound)
    {
        brain.source = sourceOfSound;
        if (brain.SeeIfSeachForSound())
        {
            brain.SetDestination(brain.source);
            TransitionToNextState(searchSoundState);            
        }
    }

    /// <summary>
    /// sees if the enemy is at the sound
    /// </summary>
    /// <returns>true if the enemy is close enough to the sound and false if not</returns>
    public bool IsAtSound()
    {
        if (brain.GetDistance(brain.source) < brain.distanceToStopFromSound)
        {
            return true;
        }
        return false;
    }
}
