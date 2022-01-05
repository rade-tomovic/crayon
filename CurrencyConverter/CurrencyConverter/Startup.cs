using CurrencyConverter.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;

namespace CurrencyConverter.API;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Currency Conversion", Version = "v1" });
            options.UseInlineDefinitionsForEnums();
        });

        services
            .AddApplicationServices();

        services.AddControllers(opt =>
        {
            opt.Filters.Add(new ProducesAttribute("application/json"));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency Conversion v1");
                c.DisplayOperationId();
            });
        }

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}