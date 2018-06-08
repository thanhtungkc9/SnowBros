using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemyWalkBehaviour : StateMachineBehaviour
{


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (GlobalControl.isPaused) return;
        animator.SetInteger("RedEnemyCurrentState", animator.GetComponent<RedEnemyAI>().STATE_WALK);
        if (animator.GetComponent<RedEnemyAI>().transform.localScale.x > 0)
        {
            float forceX = 0f;

            if (animator.GetComponent<RedEnemyAI>().redEnemyBody.velocity.x < animator.GetComponent<RedEnemyAI>().maxVelocity)
            {
                if (animator.GetComponent<RedEnemyAI>().grounded)
                {
                    forceX = animator.GetComponent<RedEnemyAI>().moveForce;
                }
                else
                {
                    forceX = animator.GetComponent<RedEnemyAI>().moveForce * 0.5f;
                }
            }
            animator.GetComponent<RedEnemyAI>().redEnemyBody.AddForce(new Vector2(forceX, 0));
        }
        else
        {
            float forceX = 0f;

            if (animator.GetComponent<RedEnemyAI>().redEnemyBody.velocity.x > -animator.GetComponent<RedEnemyAI>().maxVelocity)
            {
                if (animator.GetComponent<RedEnemyAI>().grounded)
                {
                    forceX = -animator.GetComponent<RedEnemyAI>().moveForce;
                }
                else
                {
                    forceX = -animator.GetComponent<RedEnemyAI>().moveForce * 0.5f;
                }
            }
            animator.GetComponent<RedEnemyAI>().redEnemyBody.AddForce(new Vector2(forceX, 0));
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (GlobalControl.isPaused) return;
        if (animator.GetComponent<RedEnemyAI>().transform.localScale.x > 0)
        {
            float forceX = 0f;

                if (animator.GetComponent<RedEnemyAI>().redEnemyBody.velocity.x < animator.GetComponent<RedEnemyAI>().maxVelocity)
                {
                    if (animator.GetComponent<RedEnemyAI>().grounded)
                    {
                        forceX = animator.GetComponent<RedEnemyAI>().moveForce;
                    }
                    else
                    {
                        forceX = animator.GetComponent<RedEnemyAI>().moveForce * 0.5f;
                    }
                }
                animator.GetComponent<RedEnemyAI>().redEnemyBody.AddForce(new Vector2(forceX, 0));
        }
        else
        {
            float forceX = 0f;

            if (animator.GetComponent<RedEnemyAI>().redEnemyBody.velocity.x > -animator.GetComponent<RedEnemyAI>().maxVelocity)
            {
                if (animator.GetComponent<RedEnemyAI>().grounded)
                {
                    forceX = -animator.GetComponent<RedEnemyAI>().moveForce;
                }
                else
                {
                    forceX = -animator.GetComponent<RedEnemyAI>().moveForce * 0.5f;
                }
            }
            animator.GetComponent<RedEnemyAI>().redEnemyBody.AddForce(new Vector2(forceX, 0));
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
