using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBTriggerOnExit : StateMachineBehaviour {

	public List<string> routeParameters;

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		foreach (string parameter in routeParameters) {
			animator.SetBool (Animator.StringToHash(parameter), false);
		}
	}
}
