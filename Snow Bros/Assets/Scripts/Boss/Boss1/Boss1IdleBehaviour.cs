using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1IdleBehaviour : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animator.GetComponent<AI_Boss1>().LAST_ACTION == animator.GetComponent<AI_Boss1>().STATE_JUMP)
        {
            animator.SetInteger("Boss1CurrentState", animator.GetComponent<AI_Boss1>().STATE_ATTACK);
            animator.GetComponent<AI_Boss1>().LAST_ACTION = animator.GetComponent<AI_Boss1>().STATE_ATTACK;

        }
        else
        {
            animator.SetInteger("Boss1CurrentState", animator.GetComponent<AI_Boss1>().STATE_JUMP);
            animator.GetComponent<AI_Boss1>().LAST_ACTION = animator.GetComponent<AI_Boss1>().STATE_JUMP;
        }
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      /* if ( animator.GetComponent<AI_Boss1>().LAST_ACTION== animator.GetComponent<AI_Boss1>().STATE_JUMP)
        {
            animator.SetInteger("Boss1CurrentState", animator.GetComponent<AI_Boss1>().STATE_ATTACK);
            animator.GetComponent<AI_Boss1>().LAST_ACTION = animator.GetComponent<AI_Boss1>().STATE_ATTACK;
            
        }
       else
        {
            animator.SetInteger("Boss1CurrentState", animator.GetComponent<AI_Boss1>().STATE_JUMP);
            animator.GetComponent<AI_Boss1>().LAST_ACTION = animator.GetComponent<AI_Boss1>().STATE_JUMP;
        }
        */
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
