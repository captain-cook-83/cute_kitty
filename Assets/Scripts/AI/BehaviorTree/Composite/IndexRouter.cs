using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CuteKitty")]
[TaskDescription("根据索引值选择指定任务，无论成功失败都返回结果")]
public class IndexRouter : Composite {

	public SharedInt routerIndex;

	private int currentChildIndex = 0;

	private TaskStatus executionStatus = TaskStatus.Inactive;

	public override void OnStart()
	{
		currentChildIndex = routerIndex.Value;
	}

	public override int CurrentChildIndex()
	{
		return currentChildIndex;
	}

	public override bool CanExecute()
	{
		return executionStatus == TaskStatus.Inactive;
	}

	public override void OnChildExecuted(TaskStatus childStatus)
	{
		executionStatus = childStatus;
	}

	public override void OnConditionalAbort(int childIndex)
	{
		currentChildIndex = childIndex;
		executionStatus = TaskStatus.Failure;
	}

	public override void OnEnd()
	{
		executionStatus = TaskStatus.Inactive;
	}
}
