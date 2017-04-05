using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour {

	public Transform jumpTarget;

	public float speed = 10F;

	private Rigidbody rigidbody;

	private Vector3 targetPosion;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (targetPosion != Vector3.zero) {
			Vector3 position = Vector3.Lerp (transform.position, targetPosion, Time.deltaTime * speed);
			transform.position = position;

			float distance = Vector3.Distance (position, targetPosion);
			if (distance < 0.1 || position.y > targetPosion.y) {
				rigidbody.isKinematic = false;
				targetPosion = Vector3.zero;
			}
		}
	}

	void OnJumpStartEvent() {
		targetPosion = jumpTarget.position + new Vector3(0, 0.1f, 0);
		rigidbody.isKinematic = true;
	}
}
