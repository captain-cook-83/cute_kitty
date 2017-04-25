using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovementController))]
public class CharacterAIController : MonoBehaviour {

	public int decisionIntervalSeconds = 3;

	private Animator animator;

	private CharacterMovementController movementController;

	private int currentBodyLevel;

	private float _decisionTimeGap;

	void Start () {
		animator = GetComponent<Animator> ();
		movementController = GetComponent<CharacterMovementController> ();

		animator.SetInteger (AnimatorPropertyName.GetInstance().BodyLevel, AnimatorBodyLevel.Stand);
	}

	void Update () {
		if ((_decisionTimeGap += Time.deltaTime) > decisionIntervalSeconds) {
			_decisionTimeGap = 0;

			Decide();
		}
	}

	private void Decide () {
		Debug.Log (Time.realtimeSinceStartup);
	}
}
