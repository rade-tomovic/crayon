using System.Reflection;
using CurrencyConverter.API.Middleware;
using CurrencyConverter.API.Services;
using CurrencyConverter.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;

namespace CurrencyConverter.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Currency Conversion", Version = "v1"
            });

            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        services.AddApplicationServices();
        services.AddHttpContextAccessor();

        services.AddControllers(opt =>
        {
            opt.Filters.Add(new ProducesAttribute("application/json"));
        });
        services.Configure<CurrencyServiceOptions>(options => Configuration.GetSection(CurrencyServiceOptions.SectionName).Bind(options));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
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