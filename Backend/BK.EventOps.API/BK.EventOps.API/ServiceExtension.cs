namespace BK.EventOps.API
{
    public static class ServiceExtension
    {
        public static IServiceCollection DIServiceSetup(this IServiceCollection services)
        {
            services.AddScoped<IEventOpsService, EventOpsService>();
            services.AddScoped<IEventOpsRepository, EventOpsRepository>();
            return services;
        }
    }
}