using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMovementState : StateMachineBehaviour {

	public string rootMotionParameterName = "MovementSpeed";

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Transform transform = animator.transform;
		transform.position += transform.forward * animator.GetFloat (rootMotionParameterName) * Time.deltaTime;
	}
}
