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
        animator.SetBool("playerSeen", false);
        animator.SetBool("closeEnoughToPlayer", false);
    }

    private void AttackRoutine()
    {       
        if (!brain.SeeIfPlayerIsSeen())
        {
            TransitionToNextState(searchPlayerState);           
        }
        else
        {
            brain.MonsterSpeed(false, true, false, false);
            animator.SetBool("playerSeen", true);
            if (brain.GetDistance(brain.player.transform.position) > distanceToStopFromPlayer)
            {
                brain.AssignTarget(brain.player, true);
                brain.MoveToTarget();
                animator.SetBool("closeEnoughToPlayer", false);
            }
            else
            {
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
            if (brain.GetDistance(brain.player.transform.position) < distanceMonsterCanAttackPlayerFrom)
            {
                var playerHealth = brain.player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    animator.SetBool("closeEnoughToPlayer", true);
                    playerHealth.TakeDamage();
                    brain.ResetEnemy();
                    TransitionToNextState(patrolState);
                }
            }
            timeSinceLastAttack = 0;
        }
        timeSinceLastAttack += Time.deltaTime;
    }
}
