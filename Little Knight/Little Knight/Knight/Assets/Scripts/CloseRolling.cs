using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRolling : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        Stats plStats = animator.GetComponent<Stats>();

        plStats.iframes = true;
        animator.GetComponent<PlayerController>().canMove = false;

   }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Stats plStats = animator.GetComponent<Stats>();
        PlayerAnimator pl = animator.GetComponent<PlayerAnimator>();
        PlayerController pctrl = animator.GetComponent<PlayerController>();

        plStats.iframes = false;
        pl.rolling = false;
        pl.hasDirection = false;
        animator.GetComponent<PlayerController>().canMove = false;
        animator.SetFloat("Forward", 0);


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
