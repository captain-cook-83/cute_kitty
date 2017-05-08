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

	private AudioSource audioSource;

	private float clickTime;

	void Awake() {
		behaviorTree = GetComponent<BehaviorTree> ();
		animator = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
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

	void OnBecameVisible() {
		Debug.Log (transform.name + " OnBecameVisible");
		audioSource.mute = false;
	}

	void OnBecameInvisible() {
		Debug.Log (transform.name + " OnBecameInvisible");
		audioSource.mute = true;
	}
}
