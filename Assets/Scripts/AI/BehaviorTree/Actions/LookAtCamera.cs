using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CuteKitty/Movement")]
[TaskDescription("让角色立即面向摄像机")]
public class LookAtCamera : Action {

	private Vector3 _lookAtTarget = new Vector3();

	public override void OnStart() {
		_lookAtTarget.y = transform.position.y;
	}

	public override TaskStatus OnUpdate() {
		Vector3 cameraPosition = Camera.main.transform.position;
		_lookAtTarget.x = cameraPosition.x;
		_lookAtTarget.z = cameraPosition.z;
		transform.LookAt(_lookAtTarget);
		return TaskStatus.Success;
	}
}
