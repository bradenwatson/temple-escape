using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AttackPlayer : mBrain_base
{
    public float timeInBetweenEachAttack = 1f;
    public float distanceMonsterCanAttackPlayerFrom = 3f;
    public float distanceToStopFromPlayer = 1f;

    bool paused = false;

    private float timeSinceLastAttack = 0f;

    public override void UpdateState()
    {
        if (!paused)
        {
            AttackRoutine();
        }
        else
        {
            if (!brain.player.GetComponent<PlayerHealth>().isRespawning)
            {
                ResetEnemy();
                paused = false;
            }
        }
    }

    internal override void OnStateEnterArgs()
    {
        Debug.Log("attack state");
        animator.SetBool("walking", true);
        animator.SetBool("playerSeen", false);
        animator.SetBool("closeEnoughToPlayer", false);
        brain.PlayFootSteps();
    }

    private void AttackRoutine()
    {       
        if (!brain.SeeIfPlayerIsSeen())
        {
            TransitionToNextState(searchPlayerState);           
        }
        else
        {
            brain.MonsterSpeed(chasingPlayer:true);
            animator.SetBool("closeEnoughToPlayer", false);
            animator.SetBool("playerSeen", true);
            float distance = brain.GetDistance(brain.player.transform.position);
            if (distance > distanceToStopFromPlayer && distance != -1)
            {
                brain.AssignTarget(brain.player, true);
                brain.MoveToTarget();               
                brain.PlayFootSteps();
            }
            else
            {
                brain.AssignTarget(gameObject, false);
                animator.SetBool("walking", false);
                brain.MoveToTarget();
                brain.StopFootSteps();
            }
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {        
        if (timeInBetweenEachAttack < timeSinceLastAttack && brain.player != null)
        {
            var distance = brain.GetDistance(brain.player.transform.position);
            if (distance < distanceMonsterCanAttackPlayerFrom)
            {
                var playerHealth = brain.player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    animator.SetBool("closeEnoughToPlayer", true);
                    brain.PlayerAttackingSound();
                    playerHealth.TakeDamage();
                    paused = true;
                }
            }
            timeSinceLastAttack = 0;
        }
        timeSinceLastAttack += Time.deltaTime;
    }

    private void ResetEnemy()
    {
        brain.ResetEnemy();
        TransitionToNextState(patrolState);
    }
}
