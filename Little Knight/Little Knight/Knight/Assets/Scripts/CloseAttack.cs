using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAttack : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<PlayerController>().canMove = true;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        attControl = animator.GetComponent<AttackControl>();
        attControl.currentlyAttacking = true;
        animator.GetComponent<PlayerController>().canMove = true;
        // ROTATION = TRUE / CANMOVE = FALSE
    }

    AttackControl attControl;

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

       animator.SetBool("Attack", false);
       attControl.currentlyAttacking = false;
       animator.GetComponent<PlayerController>().canMove = true;


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
