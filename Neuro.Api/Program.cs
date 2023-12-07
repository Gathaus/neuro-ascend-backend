using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Neuro.Api.Configurations;
using Neuro.Api.Middlewares;
using Neuro.Application;
using Neuro.Infrastructure;
using Neuro.Infrastructure.ApiDocumentation;
using Neuro.Infrastructure.Ef;
using Neuro.Infrastructure.Ef.Contexts;
using Neuro.Infrastructure.Logging;
using Neuro.Infrastructure.MessageBus.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Configure Serilog
// builder.Services.AddNeuroLogging(configuration.GetConnectionString("SqlServer"));

// builder.Host.UseSerilog();

builder.Services.AddInfrastructureEf(configuration.GetConnectionString("PostgreServer"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false; 
        options.Password.RequireNonAlphanumeric = false; 
        options.Password.RequireUppercase = false; 
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


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