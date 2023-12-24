using System.Text;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Neuro.Api.Configurations;
using Neuro.Api.Managers;
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
// builder.Services.AddNeuroLogging(configuration.GetConnectionString("PostgreServer"));

// builder.Host.UseSerilog();

var certificatePath = Path.Combine(AppContext.BaseDirectory, "Configurations", "ca-certificate.crt");
var postgreServerConnectionString = configuration.GetConnectionString("PostgreServer") + $"RootCertificate={certificatePath};";
var logDBConnectionString = configuration.GetConnectionString("LogDB") + $"RootCertificate={certificatePath};";

var nlogConfig = configuration.GetSection("NLog");
builder.Services.AddRemLogger(nlogConfig);


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
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
            ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))

        };
    });


builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });


builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCustomCors();

var rabbitMqConfig = configuration.GetSection("RabbitMQ");
builder.Services.ConfigureMassTransit(rabbitMqConfig["Url"]);

builder.Services.AddApplication();

builder.Services.AddInfrastructureEf(postgreServerConnectionString,
    postgreServerConnectionString);
builder.Services.AddInfrastructure();
builder.Services.AddScoped<StorageManager>(provider =>
{
    var accessKey = "DO007U98MQTMT2RLVHKE";
    var secretKey = "YcMZvxYJFUUg9CX9JQhZksmM0isKsv6Z4k8mZYOeAag";
    return new StorageManager(accessKey, secretKey);
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddNeuroSwagger();

// builder.Services.AddNeuroAuthentication(builder.Configuration);

builder.Host.UseNeuroLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
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
