using Microsoft.Extensions.DependencyInjection;
using GymManagement.Application.Services;

namespace GymManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            return services;
        }
    }
}