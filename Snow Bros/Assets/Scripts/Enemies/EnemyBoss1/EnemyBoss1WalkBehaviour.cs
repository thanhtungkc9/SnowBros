using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss1WalkBehaviour : StateMachineBehaviour
{


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animator.GetComponent<AIEnemyBoss1>().transform.localScale.x > 0)
        {
            float forceX = 0f;

                if (animator.GetComponent<AIEnemyBoss1>().enemyBoss1Body.velocity.x < animator.GetComponent<AIEnemyBoss1>().maxVelocity)
                {
                    if (animator.GetComponent<AIEnemyBoss1>().grounded)
                    {
                        forceX = animator.GetComponent<AIEnemyBoss1>().moveForce;
                    }
                    else
                    {
                        forceX = animator.GetComponent<AIEnemyBoss1>().moveForce * 0.5f;
                    }
                }
                animator.GetComponent<AIEnemyBoss1>().enemyBoss1Body.AddForce(new Vector2(forceX, 0));
        }
        else
        {
            float forceX = 0f;

            if (animator.GetComponent<AIEnemyBoss1>().enemyBoss1Body.velocity.x > -animator.GetComponent<AIEnemyBoss1>().maxVelocity)
            {
                if (animator.GetComponent<AIEnemyBoss1>().grounded)
                {
                    forceX = -animator.GetComponent<AIEnemyBoss1>().moveForce;
                }
                else
                {
                    forceX = -animator.GetComponent<AIEnemyBoss1>().moveForce * 0.5f;
                }
            }
            animator.GetComponent<AIEnemyBoss1>().enemyBoss1Body.AddForce(new Vector2(forceX, 0));
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
