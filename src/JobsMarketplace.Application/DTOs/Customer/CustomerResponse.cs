namespace JobsMarketplace.Application.DTOs.Customer
{
    public class CustomerResponse
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public DateTimeOffset CreatedAt { get; init; }
    }


}
