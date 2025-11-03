using Template.Infrastructure.Data.Contexts;
using Template.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Worker;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        // Add Entity Framework
        var connectionString = context.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value 
            ?? "Data Source=TemplateDb_Dev.db";
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        // Add MediatR
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(Template.Application.Commands.ICommand).Assembly);
        });

        // Add Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Add Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>("database")
            .AddCheck("self", () => HealthCheckResult.Healthy(), new[] { "self" });
    })
    .Build();

// Ensure database is created
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

host.Run();

// Make Program class accessible for testing
public partial class Program { }
