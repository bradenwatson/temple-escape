using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AttackPlayer : mBrain_base
{
    public float timeInBetweenEachAttack = 1f;
    private float timeSinceLastAttack = 0f;

    public override void UpdateState()
    {
        AttackRoutine();
    }

    internal override void OnStateEnterArgs()
    {
        Debug.Log("attack state");
    }

    private void AttackRoutine()
    {
        brain.AssignTarget(brain.player, true);
        brain.MoveToTarget();
        if (brain.distanceToAttackPlayer <= brain.GetDistanceToPlayer())
        {
            TransitionToNextState(searchPlayerState);
        }
        else
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if (timeInBetweenEachAttack < timeSinceLastAttack && brain.player != null)
        {
            PlayerHealthTest playerHealth = brain.player.GetComponent<PlayerHealthTest>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
            }
            timeSinceLastAttack = 0;
        }
        timeSinceLastAttack += Time.deltaTime;
    }
}
