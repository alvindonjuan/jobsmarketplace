using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Application.Interfaces.Services;
using JobsMarketplace.Infrastructure.Caching;
using JobsMarketplace.Infrastructure.Persistence;
using JobsMarketplace.Infrastructure.Persistence.Queries;
using JobsMarketplace.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobsMarketplace.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var defaultConnectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var readConnection = configuration.GetConnectionString("ReadConnection")
                    ?? throw new InvalidOperationException("Connection string 'ReadConnection' not found.");

            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));

            services.AddSingleton<IDbConnectionFactory>(new DbConnectionFactory(defaultConnectionString));

            services.AddSingleton<IReadDbConnectionFactory>(new ReadDbConnectionFactory(readConnection));

            // Redis registration
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =
                    configuration.GetConnectionString("Redis");
            });

            services.AddScoped<ICacheService, RedisCacheService>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IContractorRepository, ContractorRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IJobOfferRepository, JobOfferRepository>();

            services.AddScoped<ICustomerQuery, CustomerQuery>();
            services.AddScoped<IContractorQuery, ContractorQuery>();
            services.AddScoped<IJobQuery, JobQuery>();
            services.AddScoped<IJobOfferQuery, JobOfferQuery>();

            return services;
        }
    }
}
