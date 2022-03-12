namespace Konia.TaskManager.Model
{
    internal class Task
    {
        public Task() { }

        public Task(string title, int requirementId, double hours)
        {
            Title = title;
            RequirementId = requirementId;
            Hours = hours;
        }

        public Task(string title, int requirementId) : this(title, requirementId, 0) { }

        public string Title { get; set; }
        public int RequirementId { get; set; }

        private double _hours;
        public double Hours { get { return _hours; } set { _hours = value; } }
    }
}
