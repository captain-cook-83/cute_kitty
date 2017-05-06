using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CuteKitty/Movement")]
[TaskDescription("移动角色到指定位置")]
public class MoveToTarget : Action {

	public SharedTransform targetTransform;

	public float nearDistance = 0.15F;

	private CharacterMovementController movementController;

	private Vector3 targetPosition;

	public override void OnAwake() {
		movementController = GetComponent<CharacterMovementController> ();
	}

	public override void OnStart() {
		Transform transform = targetTransform.Value;
		if (targetPosition != transform.position) {
			targetPosition = transform.position;
			movementController.MoveToTarget(targetPosition, transform.forward);
		}
	}

	public override TaskStatus OnUpdate() {
		if (movementController.IsMoving) {
			return TaskStatus.Running;
		} else {
			if (Vector3.Distance (transform.position, targetPosition) < nearDistance) {
				return TaskStatus.Success;
			} else {
				return TaskStatus.Failure;
			}
		}
	}

	public override void OnEnd() {
		movementController.StopMoving ();
	}
}
