using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;
using Kittypath;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(JumpStateController))]
public class CharacterMovementController : MonoBehaviour {

	public bool isInteracived = false;

	public RichFunnel.FunnelSimplification funnelSimplification = RichFunnel.FunnelSimplification.None;

	public float stopDistance = 0.1F;

	public float minJumpVerticleHeight = 0.05F;

//	public float repathDistance = 0.2F;

	private Animator animator;

	private Seeker seeker;

	private JumpStateController jumpStateController;

	private bool isMoving;

	private Vector3 targetPosition = Vector3.zero;

	private Vector3 finalDirection = Vector3.zero;

	private KittyPath kittyPath;

	private PathSegment currentPathSegment;

	private Vector3 _lookAtTarget = new Vector3 ();

	public void MoveToTarget(Vector3 targetPosition, Vector3 finalDirection) {
		this.isMoving = true;
		this.targetPosition = targetPosition;
		this.finalDirection = finalDirection;
		seeker.StartPath (transform.position, targetPosition, OnPathComplete);
	}

	public void MoveToTarget(Vector3 targetPosition) {
		MoveToTarget (targetPosition, Vector3.zero);
	}

	public bool IsMoving {
		get { return isMoving; }
	}

	public void StopMoving() {
		if (isMoving) {
			animator.SetTrigger (AnimatorPropertyName.GetInstance().Movement2StopTrigger);
			animator.SetInteger (AnimatorPropertyName.GetInstance().BodyLevel, AnimatorBodyLevel.Stand);
			currentPathSegment = null;
			isMoving = false;
		}
	}

	void Start () {
		animator = GetComponent<Animator> ();
		seeker = GetComponent<Seeker> ();
		jumpStateController = GetComponent<JumpStateController> ();
	}

	private Vector3 hitPointOnButtonDown = Vector3.zero;

	void Update() {
		if (isInteracived) {
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

//		RaycastHit testHit;
//		Ray testRay = new Ray(transform.position + transform.forward * repathDistance, transform.forward);
//		if (Physics.Raycast (testRay, out testHit, repathDistance)) {
//			if (testHit.collider.CompareTag("Player")) {
//				StopMoving ();
//			}
//		}
	}

	void LateUpdate () {
		if (currentPathSegment == null || jumpStateController.IsProcessing)
			return;

		if (jumpStateController.CheckFinishedSign() || 
				VectorMath.SqrDistanceXZ (currentPathSegment.endPoint, transform.position) < stopDistance * stopDistance) {
			if (kittyPath.HasNext ()) {
				UpdateSegmentTargetAndDirection ();
			} else {
				if (Vector3.zero != finalDirection) {
					transform.LookAt (transform.position + finalDirection);
				}
				StopMoving ();
			}
		}
	}

	private float collisionTime;
	private float collisionTimeInterval = 1;
	void OnCollisionEnter(Collision collision) {
		if (IsMoving && collisionTime + collisionTimeInterval < Time.time && collision.collider.CompareTag("Player")) {
			collisionTime = Time.time;

			currentPathSegment = null;
			animator.SetTrigger (AnimatorPropertyName.GetInstance ().Movement2StopTrigger);
			CharacterMovementController collisionMovementController = collision.transform.GetComponent<CharacterMovementController> ();
			if (collisionMovementController.IsMoving) {
				StartCoroutine (RepathForCurrentTarget(transform.GetInstanceID () > collision.transform.GetInstanceID ()));
			} else {
				StartCoroutine (RepathForCurrentTarget(true));
			}
		}
	}

	private IEnumerator RepathForCurrentTarget(bool priorMove) {
		if (priorMove) {
			yield return new WaitForSeconds(0.1F);
		} else {
			animator.SetInteger (AnimatorPropertyName.GetInstance().BodyLevel, AnimatorBodyLevel.Stand);
			animator.SetBool (AnimatorPropertyName.GetInstance().TeaseBTrigger, true);
			yield return new WaitForSeconds(3);
		}

		MoveToTarget (targetPosition, finalDirection);
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
