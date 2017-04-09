using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kittypath;
using Kittyutils;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class JumpStateController : MonoBehaviour {

	private int PROPERTY_JUMPUP;
	private int PROPERTY_JUMPFORWARD;
	private int PROPERTY_JUMPDOWN;

	public string rootMotionParameterName = "MovementSpeed";

	public float timedUpwardSpeed = 10.0F;
	public float stopOffset = 0.1F;
	public float normalizedForwardDistance = 0.24F;

	private Rigidbody rigidbody;
	private Animator animator;

	private Vector3 targetPosition = Vector3.zero;
	private PathSegmentThroughStyle throughStyle = PathSegmentThroughStyle.Directly;

	private float multipleForwardSpeed = 1.0F;
	private bool isMoving = false;
	private bool isProcessing = false;

	public bool IsProcessing {
		get { return isProcessing; }
	}

	public void JumpToTarget(Vector3 targetPosition, PathSegmentThroughStyle throughStyle) {
		if (PathSegmentThroughStyle.JumpUp == throughStyle) {
			this.targetPosition = targetPosition + new Vector3 (0, 0.1F, 0);
			multipleForwardSpeed = 1.0F;
		} else {
			this.targetPosition = targetPosition;
			multipleForwardSpeed = Mathf.Max (1.0F, VectorMath.DistanceXZ (targetPosition, transform.position) / normalizedForwardDistance);
		}

		switch (throughStyle) {
		case PathSegmentThroughStyle.JumpUp:
			animator.SetTrigger (PROPERTY_JUMPUP);
			break;
		case PathSegmentThroughStyle.JumpForward:
			animator.SetTrigger (PROPERTY_JUMPFORWARD);
			break;
		case PathSegmentThroughStyle.JumpDown:
			animator.SetTrigger (PROPERTY_JUMPDOWN);
			break;
		}
		this.throughStyle = throughStyle;
		isProcessing = true;
	}

	void Awake () {
		PROPERTY_JUMPUP = Animator.StringToHash ("AnyState->JumpUp");
		PROPERTY_JUMPFORWARD = Animator.StringToHash ("AnyState->JumpForward");
		PROPERTY_JUMPDOWN = Animator.StringToHash ("AnyState->JumpDown");
	}

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
	}

	void OnAnimatorMove () {
		if (isMoving) {
			Vector3 position = transform.position;
			bool moveEnd = false;

			if (PathSegmentThroughStyle.JumpUp == throughStyle) {
				position = Vector3.Lerp (position, targetPosition, Time.deltaTime * timedUpwardSpeed);
				moveEnd = position.y > targetPosition.y;
			} else {
				position += transform.forward * animator.GetFloat(rootMotionParameterName) * Time.deltaTime * multipleForwardSpeed;
				if (PathSegmentThroughStyle.JumpDown == throughStyle) {
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

	/**
	 * 跳跃角色离开地面
	 */
	void OnJumpStartEvent() {
		isMoving = true;
		rigidbody.isKinematic = true;
	}

	/**
	 * 跳跃角色略过至高点，即将下降
	 */
	void OnJumpEndEvent() {
		rigidbody.isKinematic = false;
	}

	/**
	 * 跳跃角色动作即将结束，平面位移停止
	 */ 
	void OnJumpStopEvent() {
		isMoving = false;
	}

	/**
	 * 整个动画播放结束
	 */
	void OnJumpAnimationEnd() {
		isProcessing = false;
	}
}
