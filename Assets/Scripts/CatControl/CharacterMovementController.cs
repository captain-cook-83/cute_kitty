using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;
using Kittypath;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(JumpStateController))]
public class CharacterMovementController : MonoBehaviour {

	public Transform moveTarget;

	public RichFunnel.FunnelSimplification funnelSimplification = RichFunnel.FunnelSimplification.None;

	public float stopDistance = 0.03F;

	public float minJumpVerticleHeight = 0.05F;

	private Animator animator;

	private Seeker seeker;

	private JumpStateController jumpStateController;

	private KittyPath kittyPath;

	private PathSegment currentPathSegment;

	private Vector3 _lookAtTarget = new Vector3 ();

	public void MoveToTarget(Vector3 targetPosition) {
		seeker.StartPath (transform.position, targetPosition, OnPathComplete);
	}

	void Start () {
		animator = GetComponent<Animator> ();
		seeker = GetComponent<Seeker> ();
		jumpStateController = GetComponent<JumpStateController> ();

		if (moveTarget != null && moveTarget.gameObject.activeSelf) {			// 方便场景测试
			MoveToTarget (moveTarget.position);
		}
	}

	private Vector3 hitPointOnButtonDown = Vector3.zero;

	void Update() {
		RaycastHit hit;

		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 100)) {
				hitPointOnButtonDown = hit.point;
			}
		} else if (Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 100)) {
				if (hit.collider.CompareTag ("Environments") && Vector3.Distance (hit.point, hitPointOnButtonDown) < 0.05) {
					MoveToTarget(hit.point);
				}
			}
		}
	}

	void LateUpdate () {
		if (currentPathSegment == null || jumpStateController.IsProcessing)
			return;

		if (jumpStateController.CheckFinishedSign() || 
				VectorMath.SqrDistanceXZ (currentPathSegment.endPoint, transform.position) < stopDistance * stopDistance) {
			if (kittyPath.HasNext ()) {
				UpdateSegmentTargetAndDirection ();
			} else {
				animator.SetInteger (AnimatorPropertyName.GetInstance().BodyLevel, AnimatorBodyLevel.Stand);
				currentPathSegment = null;
			}
		}
	}

	private void OnPathComplete(Path path) {
		path.Claim (this);
		if (!path.error) {
			kittyPath = new KittyPath (path.vectorPath, minJumpVerticleHeight, jumpStateController.MaxForwardDistance);
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
		_lookAtTarget.y = transform.position.y;
		transform.LookAt(_lookAtTarget);

		//设置角色动画参数
		if (PathSegmentThroughStyle.Directly == currentPathSegment.throughStyle) {
			animator.SetInteger (AnimatorPropertyName.GetInstance().BodyLevel, AnimatorBodyLevel.Trot);
		} else {
			jumpStateController.JumpToTarget (currentPathSegment.endPoint, currentPathSegment.throughStyle);
		}
	}
}
