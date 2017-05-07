using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;

public class CharacterInteractionController : MonoBehaviour {

	public float doubleClickInterval = 1F;

	public string doubleClickEventName = "DoubleClicked";

	public string clickEventName = "Clicked";

	public float maxInteractiveDistance = 1.6F;

	private BehaviorTree behaviorTree;

	private Animator animator;

	private float clickTime;

	void Awake() {
		behaviorTree = GetComponent<BehaviorTree> ();
		animator = GetComponent<Animator> ();
	}

	void OnMouseDown() {
		Vector3 direction = transform.position - Camera.main.transform.position;
		if (direction.magnitude < maxInteractiveDistance) {
			if (animator.GetBool (AnimatorPropertyName.GetInstance ().InteractiveMode)) {
				behaviorTree.SendEvent (clickEventName);
			} else {
				if (Time.realtimeSinceStartup - clickTime < doubleClickInterval) {
					Debug.Log ("BehaviorTree Event: " + doubleClickEventName);
					behaviorTree.SendEvent (doubleClickEventName);
				} else {
					clickTime = Time.realtimeSinceStartup;
				}
			}
		}
	}
}
