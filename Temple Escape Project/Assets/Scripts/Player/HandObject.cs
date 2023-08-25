using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Animator))]
public class HandObject : MonoBehaviour
{
    public float speed = 10;
    private Animator animator;

    private string gripAnimatorName = "Grip";
    private float gripTarget;
    private float gripCurrent;

    private string triggerAnimatorName = "Trigger";
    private float triggerTarget;
    private float triggerCurrent;

    private string teleportAnimatorName = "Teleport";
    private float teleportTarget;
    private float teleportCurrent;

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

        if (teleportCurrent != teleportTarget)
        {
            teleportCurrent = Mathf.MoveTowards(teleportCurrent, teleportTarget, Time.deltaTime * speed);
            animator.SetFloat(teleportAnimatorName, teleportCurrent);
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

    internal void SetTeleport(float value)
    {
       teleportTarget = value;
    }
}