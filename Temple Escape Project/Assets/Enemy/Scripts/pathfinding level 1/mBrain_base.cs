using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class mBrain_base : MonoBehaviour
{
    public bool isActive = false;
    internal mBrain_brain brain;

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
}
