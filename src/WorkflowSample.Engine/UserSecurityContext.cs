namespace WorkflowSample.Engine
{
    public class UserSecurityContext : IUserSecurityContext
    {
        public User CurrentUser { get; set; }
    }
}