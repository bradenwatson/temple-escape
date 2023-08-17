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

    public void OnStateEnter()
    {
        isActive = true;
        OnStateEnterArgs();
    }

    public void AssignBrain(mBrain_brain brain)
    {
        this.brain = brain;
    }

    internal virtual void OnStateEnterArgs()
    {

    }

    public abstract void UpdateState();

    public void TransitionToNextState(mBrain_base nextState)
    {
        OnStateExit();
        brain.RecieveNewState(nextState);
        nextState.OnStateEnter();
        
    }

    void OnStateExit() 
    { 
        isActive = false;
        OnStateExitArgs();
    }

    internal virtual void OnStateExitArgs()
    {

    }

    public void AssignStates(mBrain_base patrolState, mBrain_base attackState, mBrain_base searchForPlayer, mBrain_base checkCollectableState)
    {
        this.patrolState = patrolState;
        this.attackState = attackState;
        this.searchPlayerState = searchForPlayer;
        this.searchCollectibleState = checkCollectableState;
        
    }

    public bool SeeIfPlayerSeen()
    {
        if (brain.distanceToAttackPlayer > brain.GetDistanceToPlayer())
        {
            return true;
        }
        return false;
    }
}
