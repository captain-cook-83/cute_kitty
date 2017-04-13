using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kittypath;
using Kittyutils;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class JumpStateController : MonoBehaviour {

	private const float MAX_SPEED_MULTIPLE = 2.0F;

	private int PROPERTY_JUMPUP;
	private int PROPERTY_JUMPFORWARD;
	private int PROPERTY_JUMPDOWN;

	public string rootMotionParameterName = "MovementSpeed";

	public float timedUpwardSpeed = 10.0F;
	public float normalizedForwardDistance = 0.4F;

	private Rigidbody rigidbody;
	private Animator animator;

	private Vector3 targetPosition = Vector3.zero;
	private PathSegmentThroughStyle throughStyle = PathSegmentThroughStyle.Directly;

	private float multipleForwardSpeed = 1.0F;
	private bool isMoving = false;
	private bool isProcessing = false;
	private bool finishedSign = false;

	/**
	 * 单词前向跳跃的最大距离
	 */ 
	public float MaxForwardDistance {
		get { return normalizedForwardDistance * MAX_SPEED_MULTIPLE; }
	}

	/**
	 * 检查跳跃动作是否正在处理之中
	 */
	public bool IsProcessing {
		get { return isProcessing; }
	}

	/**
	 * 检查并重置跳跃完成标记
	 */ 
	public bool CheckFinishedSign() {
		if (finishedSign) {
			finishedSign = false;
			return true;
		} else {
			return false;
		}
	}

	/**
	 * 控制角色从当前位置向指定位置跳跃
	 * 
	 * \param targetPosition 目标位置（全局坐标系）
	 * \param throughStyle 通过方式，默认指定为向前跳跃
	 */ 
	public void JumpToTarget(Vector3 targetPosition, PathSegmentThroughStyle throughStyle) {
		if (PathSegmentThroughStyle.JumpUp == throughStyle) {
			this.targetPosition = targetPosition + new Vector3 (0, 0.02F, 0);
			multipleForwardSpeed = 1.0F;
		} else {
			this.targetPosition = targetPosition;
			multipleForwardSpeed = VectorMath.DistanceXZ (targetPosition, transform.position) / normalizedForwardDistance;
			multipleForwardSpeed = Mathf.Clamp (multipleForwardSpeed, 1.0F, MAX_SPEED_MULTIPLE);
		}

		switch (throughStyle) {
		case PathSegmentThroughStyle.JumpUp:
			animator.SetTrigger (PROPERTY_JUMPUP);
			break;
		case PathSegmentThroughStyle.JumpDown:
			animator.SetTrigger (PROPERTY_JUMPDOWN);
			break;
		default:
			animator.SetTrigger (PROPERTY_JUMPFORWARD);
			break;
		}

		this.throughStyle = throughStyle;

		isProcessing = true;
		finishedSign = false;
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

			if (moveEnd) {
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
		finishedSign = true;
	}
}
