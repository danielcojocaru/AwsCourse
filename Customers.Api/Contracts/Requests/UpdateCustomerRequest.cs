using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Contracts.Requests;

public class UpdateCustomerRequest
{
    [FromRoute]
    public Guid Id { get; init; }

    [FromBody]
    public CustomerRequest Customer { get; set; } = default!;
}
