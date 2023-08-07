using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HandObject : MonoBehaviour
{
    private Animator animator;
    private string gripAnimatorName = "Grip";
    private string triggerAnimatorName = "Trigger";

    private float gripTarget;
    private float gripCurrent;
    private float triggerTarget;
    private float triggerCurrent;
    public float speed = 10;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void AnimateHand()
    {
        if (gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            animator.SetFloat(gripAnimatorName, gripCurrent);
        }

        if (triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            animator.SetFloat(triggerAnimatorName, triggerCurrent);
        }
    }

    private void Update()
    {
        AnimateHand();
    }

    internal void SetGrip(float value)
    {
        gripTarget = value;
    }

    internal void SetTrigger(float value)
    {
        triggerTarget = value;
    }
}