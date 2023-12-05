using Neuro.Api.Configurations;
using Neuro.Api.Middlewares;
using Neuro.Application;
using Neuro.Infrastructure;
using Neuro.Infrastructure.ApiDocumentation;
using Neuro.Infrastructure.Ef;
using Neuro.Infrastructure.Logging;
using Neuro.Infrastructure.MessageBus.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Configure Serilog
builder.Services.AddNeuroLogging(configuration.GetConnectionString("SqlServer"));

builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });


builder.Services.AddControllers();

builder.Services.AddCustomCors();

var rabbitMqConfig = configuration.GetSection("RabbitMQ");
builder.Services.ConfigureMassTransit(rabbitMqConfig["Url"]);

builder.Services.AddApplication();
builder.Services.AddInfrastructureEf(configuration.GetConnectionString("PostgreServer"));

builder.Services.AddInfrastructure();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddNeuroSwagger();

// builder.Services.AddNeuroAuthentication(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();