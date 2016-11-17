namespace WorkflowSample.Engine
{
    public interface IUserSecurityContext
    {
        User CurrentUser { get; set; }
    }
}