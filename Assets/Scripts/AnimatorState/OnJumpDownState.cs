using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnJumpDownState : StateMachineBehaviour {

	public string rootMotionParameterName = "MovementSpeed";

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//		animator.GetComponent<Rigidbody> ().isKinematic = true;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (animator.GetFloat(rootMotionParameterName) > 0) {
			Transform transform = animator.transform;
			transform.position = transform.position + transform.forward * 0.5F * Time.deltaTime;
		}
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//		animator.GetComponent<Rigidbody> ().isKinematic = false;
	}
}
