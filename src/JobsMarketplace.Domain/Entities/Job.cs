using JobsMarketplace.Domain.Enums;


namespace JobsMarketplace.Domain.Entities
{
    public class Job
    {
        public Guid Id { get; private set; }

        public Guid CustomerId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public decimal Budget { get; private set; }

        public JobStatus Status { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        public DateTimeOffset? StartDate { get; private set; }

        public DateTimeOffset? EndDate { get; private set; }
        public DateTimeOffset? DueDate { get; private set; }

        private Job() { }

        public Job(Guid customerId, string title, string description,
                   decimal budget)
        {
            if (budget <= 0)
                throw new ArgumentException("Budget must be greater than zero");

            Id = Guid.NewGuid();
            CustomerId = customerId;
            Title = title;
            Description = description;
            Budget = budget;
            Status = JobStatus.Open;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateJob(string title, string description, decimal budget)
        {
            if (budget <= 0)
                throw new ArgumentException("Budget must be greater than zero");

            Title = title;
            Description = description;
            Budget = budget;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Start()
        {
            Status = JobStatus.InProgress;
            StartDate = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Complete()
        {
            Status = JobStatus.Completed;
            EndDate = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Cancel()
        {
            Status = JobStatus.Cancelled;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }

}
