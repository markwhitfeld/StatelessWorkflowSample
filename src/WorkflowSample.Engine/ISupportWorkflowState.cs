namespace WorkflowSample.Engine
{
    public interface ISupportWorkflowState<TStatus>
    {
        TStatus Status { get; }
        void SetStatus(TStatus status);
    }
}