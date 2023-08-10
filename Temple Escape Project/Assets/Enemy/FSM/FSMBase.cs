using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMBase : MonoBehaviour
{
    internal Animator brain;

    //when state is entered
    public abstract void Invoke(Animator _brain);
    //when state is exited
    public abstract void Stop();

    internal void ActionComplete()
    {
        brain.SetTrigger("ActionComplete");
    }

    internal void ResetActionComplete()
    {
        brain.ResetTrigger("ActionComplete");
    }




    // door control
    public abstract void DoorControl();
    // 

}
