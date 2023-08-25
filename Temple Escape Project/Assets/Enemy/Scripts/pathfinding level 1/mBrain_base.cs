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

    public void AssignStates(mBrain_base patrolState, mBrain_base attackState, mBrain_base searchForPlayer, mBrain_base checkCollectableState, mBrain_base searchSoundState)
    {
        this.patrolState = patrolState;
        this.attackState = attackState;
        this.searchPlayerState = searchForPlayer;
        this.searchCollectibleState = checkCollectableState;
        this.searchSoundState = searchSoundState;
    }

    public bool SeeIfPlayerSeen()
    {
        if (brain.distanceToAttackPlayer > brain.GetDistanceToPlayer())
        {
            return true;
        }
        return false;
    }

    public void SearchSound(Vector3 sourceOfSound)
    {
        brain.source = sourceOfSound;
        print(brain.source);
        if (brain.SeeIfSeachForSound())
        {
            brain.SetDestination(brain.source);
            TransitionToNextState(searchSoundState);            
        }
    }

    public bool IsAtSound()
    {
        print(brain.source);
        if (brain.GetDistance(brain.source) < 10f)
        {
            return true;
        }

        return false;
    }
}
