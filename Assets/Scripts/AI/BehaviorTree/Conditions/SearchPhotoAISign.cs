using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CuteKitty/Search")]
[TaskDescription("搜索场景内可以观看的照片")]
public class SearchPhotoAISign : Conditional {

	public AISignsHolder aiSignsHolder;

	public SharedTransform lookAtTarget;

	public override void OnAwake() {
		
	}

	public override TaskStatus OnUpdate() {
		AISign aiSign = aiSignsHolder.GetRandomPhotoSign ();
		if (aiSign != null) {
			lookAtTarget.SetValue (aiSign.transform);
			return TaskStatus.Success;
		} else {
			lookAtTarget.SetValue (null);
			return TaskStatus.Failure;
		}
	}
}
