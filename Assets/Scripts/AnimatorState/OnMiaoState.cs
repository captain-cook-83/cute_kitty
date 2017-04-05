using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMiaoState : StateMachineBehaviour {

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Transform transform = animator.transform;
		AudioSource aidioSource = transform.GetComponent<AudioSource> ();
		aidioSource.Play ();
	}
}
