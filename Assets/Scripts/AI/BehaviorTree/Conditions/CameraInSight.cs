using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CuteKitty/Search")]
[TaskDescription("判断镜头是否在视野中")]
public class CameraInSight : Conditional {

	public float maxHorizontalFieldOfView = 15F;

	public float minUpwardFieldOfUpView = 30;

	public float checkInterval = 1F;

	public SharedBool enableLookAt;

	private Transform cameraTransform;

	private Vector2 __cameraDirection = new Vector2();

	private Vector2 __selfDirection = new Vector2();

	private float tickTime;

	public override void OnStart() {
		cameraTransform = Camera.main.transform;
	}

	public override TaskStatus OnUpdate() {
		if (tickTime > checkInterval) {
			tickTime = 0;

			Vector3 cameraDirection = cameraTransform.position - transform.position;
			Vector3 selfDirection = transform.forward;

			__cameraDirection.x = cameraDirection.x;
			__cameraDirection.y = cameraDirection.z;
			__selfDirection.x = selfDirection.x;
			__selfDirection.y = selfDirection.z;

			if (Vector2.Angle (__cameraDirection, __selfDirection) < maxHorizontalFieldOfView) {
				__cameraDirection.x = cameraDirection.z;
				__cameraDirection.y = cameraDirection.y;
				__selfDirection.x = selfDirection.z;
				__selfDirection.y = selfDirection.y;
				enableLookAt.Value = Vector2.Angle (__cameraDirection, __selfDirection) > minUpwardFieldOfUpView;
			} else {
				enableLookAt.Value = false;
			}

			Debug.Log (transform.name + " -> I " + (enableLookAt.Value ? "can" : " can not") + " see you.");
			return TaskStatus.Success;
		} else {
			tickTime += Time.deltaTime;
			return TaskStatus.Running;
		}
	}

	public override void OnEnd() {
		cameraTransform = null;
	}
}
