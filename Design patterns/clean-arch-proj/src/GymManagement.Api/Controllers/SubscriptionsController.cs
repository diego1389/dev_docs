using ErrorOr;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase{
    private readonly ISender _mediator;

    public SubscriptionsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request){
        
        var command = new CreateSubscriptionCommand(
            request.SubscriptionType.ToString(), 
            request.AdminId);

        var createSubscriptionResult = await _mediator.Send(command);

        /*if(createSubscriptionResult.IsError)
            return Problem();

        var response = new SubscriptionResponse(
            createSubscriptionResult.Value, 
            request.SubscriptionType
        );
        return Ok(response);*/
        return createSubscriptionResult.MatchFirst(
            subscription => Ok(new SubscriptionResponse(subscription.Id, request.SubscriptionType)),
            Error => Problem());
    }
}