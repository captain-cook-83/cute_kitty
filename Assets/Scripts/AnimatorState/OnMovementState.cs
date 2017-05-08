using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMovementState : StateMachineBehaviour {

	public string rootMotionParameterName = "MovementSpeed";

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Transform transform = animator.transform;
		float stepDistance = animator.GetFloat (rootMotionParameterName) * animator.speed * Time.deltaTime;
		transform.position += transform.forward * stepDistance;
	}
}
