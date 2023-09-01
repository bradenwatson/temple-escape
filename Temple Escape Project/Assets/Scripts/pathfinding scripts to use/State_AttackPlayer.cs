using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AttackPlayer : mBrain_base
{
    public float timeInBetweenEachAttack = 1f;
    public float distanceMonsterCanAttackPlayerFrom = 3f;
    public float distanceToStopFromPlayer = 1f;
    private float timeSinceLastAttack = 0f;

    public override void UpdateState()
    {
        AttackRoutine();
    }

    internal override void OnStateEnterArgs()
    {
        Debug.Log("attack state");
        animator.SetBool("walking", true);
        animator.SetBool("playerFound", true);
        animator.SetBool("stopped", false);
        animator.SetBool("closeEnoughToPlayer", false);
    }

    private void AttackRoutine()
    {       
        if (!brain.SeeIfPlayerIsSeen())
        {
            TransitionToNextState(searchPlayerState);
            animator.SetBool("playerFound", false);
        }
        else
        {
            animator.SetBool("playerFound", true);
            if (brain.GetDistance(brain.player.transform.position) > distanceToStopFromPlayer)
            {
                animator.SetBool("closeEnoughToPlayer", true);
                brain.AssignTarget(brain.player, true);
                brain.MoveToTarget();
            }
            else
            {
                animator.SetBool("closeEnoughToPlayer", false);
                brain.AssignTarget(gameObject, false); 
                brain.MoveToTarget();
            }
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if (timeInBetweenEachAttack < timeSinceLastAttack && brain.player != null)
        {
            var playerHealth = brain.player.GetComponent<DoorControl>(); // replace with player health script just using a random script
            if (playerHealth != null && brain.GetDistance(brain.player.transform.position) < distanceMonsterCanAttackPlayerFrom)
            {
                animator.SetBool("closeEnoughToPlayer", true);
                animator.SetBool("stopped", true);
                // playerHealth.TakeDamage();
            }
            else
            {
                animator.SetBool("closeEnoughToPlayer", false);
                animator.SetBool("stopped", false);
            }
            timeSinceLastAttack = 0;
        }
        timeSinceLastAttack += Time.deltaTime;
    }
}
