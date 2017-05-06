using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CuteKitty/Search")]
[TaskDescription("搜索场景内可以主动寻找的点")]
public class SearchAISign : Conditional {

	public string aiSignsTagName;

	public SharedString aiSignLabelName;

	public SharedTransform lookAtTarget;

	private AISignsHolder aiSignsHolder;

	public override void OnAwake() {
		GameObject aiSignsHolderGO = GameObject.FindWithTag (aiSignsTagName);
		if (aiSignsHolderGO) {
			aiSignsHolder = aiSignsHolderGO.GetComponent<AISignsHolder> ();
		} else {
			Debug.Log ("缺少AISigns标记对象");
		}
	}

	public override TaskStatus OnUpdate() {
		AISign aiSign = aiSignsHolder.getRandomAISign (aiSignLabelName.Value);
		if (aiSign != null) {
			lookAtTarget.SetValue (aiSign.transform);
			return TaskStatus.Success;
		} else {
			lookAtTarget.SetValue (null);
			return TaskStatus.Failure;
		}
	}
}
