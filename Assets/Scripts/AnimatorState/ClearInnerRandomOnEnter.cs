using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearInnerRandomOnEnter : StateMachineBehaviour {

	public List<string> routeParameters;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		foreach (string parameter in routeParameters) {
			animator.SetInteger (Animator.StringToHash(parameter), 0);
		}	
	}
}
