
using JobsMarketplace.Application.Interfaces.Services;
using JobsMarketplace.Application.Services;
using JobsMarketplace.Infrastructure.DependencyInjection;

namespace JobsMarketplace.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IContractorService, ContractorService>();
            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<IJobOfferService, JobOfferService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
