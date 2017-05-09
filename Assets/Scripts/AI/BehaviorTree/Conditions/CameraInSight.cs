using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CuteKitty/Search")]
[TaskDescription("判断镜头是否在视野中")]
public class CameraInSight : Conditional {

	public float maxDetectDistance = 3;

	public float maxHorizontalFieldOfView = 15;

	public float minUpwardFieldOfUpView = 30;

	public float checkInterval = 0.25F;

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

			bool canSeeCamera = false;
			Vector3 cameraDirection = cameraTransform.position - transform.position;
			if (cameraDirection.magnitude < maxDetectDistance) {
				__cameraDirection.x = cameraDirection.x;
				__cameraDirection.y = cameraDirection.z;
				__selfDirection.x = transform.forward.x;
				__selfDirection.y = transform.forward.z;

				if (Vector2.Angle (__cameraDirection, __selfDirection) < maxHorizontalFieldOfView) {
					canSeeCamera = Vector3.Angle (cameraDirection, transform.up) < 90 - minUpwardFieldOfUpView;
				}
			}

			enableLookAt.Value = canSeeCamera;
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
