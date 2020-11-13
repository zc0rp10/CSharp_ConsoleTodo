namespace ConsoleTodo
{
    class Task
    {
        public Task(int taskId, int taskPriority, string taskName)
        {
            this.taskId = taskId;
            this.taskPriority = taskPriority;
            this.taskName = taskName;
        }
        public int taskId { get; set; }
        public int taskPriority { get; set; }
        public string taskName { get; set; }
    }
}
