using System;

namespace WorkflowSample.Engine
{
    public class User
    {
        public string Name { get; }

        public User(string name = "unspecified")
        {
            Name = name;
        }
    }
}