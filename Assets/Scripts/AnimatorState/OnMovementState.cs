using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMovementState : StateMachineBehaviour {

	public string rootMotionParameterName = "MovementSpeed";

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Transform transform = animator.transform;
		float speedParameter = animator.GetFloat (rootMotionParameterName);
		transform.position += transform.forward * speedParameter * Time.deltaTime;
	}
}
