using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CuteKitty/Search")]
[TaskDescription("按照权重信息决策需要主动寻找的点信息，包括点的类型和停留时间")]
public class WeightRandomStayTarget : Conditional {

	public SharedInt taskRouterIndex;

	public SharedString staySignLabel;

	public SharedFloat stayTimeMin;

	public SharedFloat stayTimeMax;

	public List<StayTarget> stayTargets;

	private int totalWeight;

	public override void OnStart() {
		totalWeight = 0;
		foreach (StayTarget target in stayTargets) {
			totalWeight += target.weight;
		}
	}

	public override TaskStatus OnUpdate() {
		int randomValue = Random.Range(0, totalWeight);
		foreach (StayTarget target in stayTargets) {
			if (randomValue < target.weight) {
				taskRouterIndex.Value = target.routerIndex;
				staySignLabel.Value = target.staySignLabel;
				stayTimeMin.Value = target.stayTimeMin;
				stayTimeMax.Value = target.stayTimeMax;
				break;
			} else {
				randomValue -= target.weight;
			}
		}
		return TaskStatus.Success;
	}
}
