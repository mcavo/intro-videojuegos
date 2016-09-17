using UnityEngine;
using System.Collections;

public class PacmanDeathStateBehavior : StateMachineBehaviour {

	private PacmanController pacmanController = null;
	private static int death = Animator.StringToHash("Death");

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!pacmanController) {
			pacmanController = GameObject.Find ("Pacman").GetComponent<PacmanController> ();
		}

		if (stateInfo.shortNameHash == death) {
			//animator.applyRootMotion (false);
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.shortNameHash == death && stateInfo.normalizedTime > stateInfo.length) {
			pacmanController.Reset ();
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.shortNameHash == death) {
			//animator.applyRootMotion (true);
		}
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
