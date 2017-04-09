using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;
using Kittypath;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Seeker))]
public class CharacterMovementController : MonoBehaviour {

	public Transform moveTarget;

	public Transform jumpTarget;

	public RichFunnel.FunnelSimplification funnelSimplification = RichFunnel.FunnelSimplification.None;

	public float stopDistance = 0.02F;

	private Animator animator;

	private Seeker seeker;

	private KittyPath kittyPath;

	private PathSegment currentPathSegment;

	private int propertyBodyLevel;
	private int propertyJumpUp;
	private int propertyJumpForward;
	private int propertyJumpDown;

	private Vector3 _lookAtTarget = new Vector3 ();

	void Start () {
		animator = GetComponent<Animator> ();
		seeker = GetComponent<Seeker> ();

		propertyBodyLevel = Animator.StringToHash ("BodyLevel");
		propertyJumpUp = Animator.StringToHash ("AnyState->JumpUp");
		propertyJumpForward = Animator.StringToHash ("AnyState->JumpForward");
		propertyJumpDown = Animator.StringToHash ("AnyState->JumpDown");

		if (moveTarget != null) {
			MoveToTarget (moveTarget.position);
		}
	}

	void Update () {
		if (currentPathSegment == null)
			return;

		if (VectorMath.SqrDistanceXZ (currentPathSegment.endPoint, transform.position) < stopDistance * stopDistance) {
			if (kittyPath.HasNext ()) {
				UpdateSegmentTargetAndDirection ();
			} else {
				animator.SetInteger (propertyBodyLevel, AnimationBodyLevel.Stand);
				currentPathSegment = null;
			}
		}
	}

	public void MoveToTarget(Vector3 targetPosition) {
		seeker.StartPath (transform.position, targetPosition, OnPathComplete);
	}

	private void OnPathComplete(Path path) {
		path.Claim (this);
		if (!path.error) {
			kittyPath = new KittyPath (path.vectorPath);
			if (kittyPath.HasNext ()) {
				UpdateSegmentTargetAndDirection ();
			}
		}
		path.Release (this);
	}

	private void UpdateSegmentTargetAndDirection() {
		currentPathSegment = kittyPath.Next ();

		// 调整角色方向
		_lookAtTarget.x = currentPathSegment.endPoint.x;
		_lookAtTarget.z = currentPathSegment.endPoint.z;
		transform.LookAt(_lookAtTarget);

		//设置角色动画参数
		if (PathSegmentThroughStyle.Directly == currentPathSegment.throughStyle) {
			animator.SetInteger (propertyBodyLevel, AnimationBodyLevel.Trot);
		} else {
			jumpTarget.position = currentPathSegment.endPoint;
			jumpTarget.gameObject.SetActive (true);

			switch (currentPathSegment.throughStyle) {
			case PathSegmentThroughStyle.JumpUp:
				animator.SetTrigger (propertyJumpUp);
				break;
			case PathSegmentThroughStyle.JumpForward:
				animator.SetTrigger (propertyJumpForward);
				break;
			case PathSegmentThroughStyle.JumpDown:
				animator.SetTrigger (propertyJumpDown);
				break;
			}
		}
	}

	class AnimationBodyLevel {
		
		public const int Stand = 0;

		public const int Walk = 1;

		public const int Trot = 2;

		public const int Run = 3;
	}
}
