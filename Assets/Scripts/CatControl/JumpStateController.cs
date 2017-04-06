using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStateController : MonoBehaviour {

	public Transform jumpTarget;

	public string rootMotionParameterName = "MovementSpeed";

	public float timedUpwardSpeed = 10F;

	public float minVerticalJumpHeight = 0.1F;
	public float stopOffset = 0.1F;
	public float normalizedForwardDistance = 0.24F;

	private Rigidbody rigidbody;
	private Animator animator;

	private Vector3 targetPosition = Vector3.zero;
	private Vector3 targetDirection = Vector3.zero;

	private float multipleForwardSpeed = 1.0F;
	private bool isJumpUpward = false;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
	}

	void OnAnimatorMove () {
		if (targetPosition != Vector3.zero) {
			Vector3 position = transform.position;
			bool moveEnd = false;

			if (isJumpUpward) {
				position = Vector3.Lerp (position, targetPosition, Time.deltaTime * timedUpwardSpeed);
				moveEnd = position.y > targetPosition.y;
			} else {
				position.z += animator.GetFloat(rootMotionParameterName) * Time.deltaTime * multipleForwardSpeed;
				if (targetDirection.y <= - minVerticalJumpHeight) {
					moveEnd = position.y <= targetPosition.y;
				}
			}

			transform.position = position;
			if (moveEnd || Vector3.Distance (position, targetPosition) < stopOffset) {		// 可以考虑去除的判断
				OnJumpEndEvent ();
				OnJumpStopEvent ();
			}
		}
	}

	void OnJumpStartEvent() {
		rigidbody.isKinematic = true;

		if (jumpTarget.gameObject.activeSelf) {
			targetPosition = jumpTarget.position;
		} else {
			targetPosition = transform.position + transform.forward * normalizedForwardDistance;
		}

		targetDirection = targetPosition - transform.position;
		isJumpUpward = targetDirection.y >= minVerticalJumpHeight;

		if (isJumpUpward) {
			multipleForwardSpeed = 1.0F;
			targetPosition += new Vector3(0, 0.1F, 0);
		} else {
			multipleForwardSpeed = Mathf.Max (1.0F, Mathf.Abs(targetDirection.z) / normalizedForwardDistance);
		}
	}

	void OnJumpEndEvent() {
		rigidbody.isKinematic = false;
	}

	void OnJumpStopEvent() {
		targetPosition = Vector3.zero;
	}
}
