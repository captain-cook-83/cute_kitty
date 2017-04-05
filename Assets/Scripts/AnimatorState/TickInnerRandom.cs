using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickInnerRandom : StateMachineBehaviour {

	public string innerRandomProperty = "Inner->Random";

	public int rangValue = 30;

	public float intervalSeconds = 3;

	private float tickTime = 0;

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (tickTime > intervalSeconds) {
			tickTime = 0;
			animator.SetInteger (Animator.StringToHash (innerRandomProperty), Random.Range (1, rangValue));
		} else {
			tickTime += Time.deltaTime;
		}
	}
}
