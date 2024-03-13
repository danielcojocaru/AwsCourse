using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers;

public class CustomerDeletedHandler : IRequestHandler<CustomerDeleted>
{
    private readonly ILogger<CustomerDeletedHandler> _logger;

    public CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger)
    {
        _logger = logger;
    }

    public Task<Unit> Handle(CustomerDeleted request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Customer Deleted: {request.Id}");
        return Unit.Task;
    }
}
