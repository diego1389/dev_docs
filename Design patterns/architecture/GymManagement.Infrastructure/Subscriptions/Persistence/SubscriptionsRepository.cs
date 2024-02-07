using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Common.Persistence;

namespace GymManagement.Infrastructure.Subscriptions.Persistence;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly GymManagementDbContext _dbContext;

    public SubscriptionsRepository(GymManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }
   
    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        //Add the subscription to the database
        await _dbContext.Subscriptions.AddAsync(subscription);
    }

    public async Task<Subscription?> GetByIdAsync(Guid subscriptionId)
    {
        //Get the subscription from the database
        var subscription = await _dbContext.Subscriptions.FindAsync(subscriptionId);
        return subscription;
    }
}