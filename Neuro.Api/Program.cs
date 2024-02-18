using System.Text;
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
using Neuro.Infrastructure.Hangfire;
using Neuro.Infrastructure.MessageBus.Configuration;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
try
{
// Configure Serilog
// builder.Services.AddNeuroLogging(configuration.GetConnectionString("PostgreServer"));

// builder.Host.UseSerilog();

    var certificatePath = Path.Combine(AppContext.BaseDirectory, "Configurations", "ca-certificate.crt");
    var postgreServerConnectionString =
        configuration.GetConnectionString("PostgreServer") + $"RootCertificate={certificatePath};";
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
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
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

    if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
    {
        builder.Services.AddHangfireServices(builder.Configuration.GetConnectionString("PostgreServer")!);
    }

    // if (builder.Environment.IsDevelopment())
    // {
    //     builder.Services.AddHangfireServices(builder.Configuration.GetConnectionString("PostgreServer")!);
    // }


    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    if (app.Environment.IsDevelopment())
    {
    }

    if (app.Environment.IsStaging() || app.Environment.IsProduction())
    {
        app.UseCustomHangfireDashboard(app.Services);
        HangfireConfiguration.RestartProcessingJobsBeforeStartingServer(app.Services);
    }

    if (builder.Environment.IsDevelopment())
    {
        // app.UseCustomHangfireDashboard(app.Services);
        // HangfireConfiguration.RestartProcessingJobsBeforeStartingServer(app.Services);
    }

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseCors("AllowAllOrigins");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // Hata yakalandığında çalışacak kod
    var connectionString =
        "Host=db-postgresql-nyc3-48272-do-user-15119865-0.c.db.ondigitalocean.com;Port=25060;Database=neuro_ascend_mvp;Username=doadmin;Password=AVNS_AZDUg45ahNV9RWG_cQT;SslMode=VerifyCA;Trust Server Certificate=false;RootCertificate=./ca-certificate.crt;";
    using (var connection = new NpgsqlConnection(connectionString))
    {
        var cmdText =
            @"INSERT INTO logs (userid, email, userfullname, ipaddress, deviceid, version, datetime, errorcode, level, caller, userfriendlymessage, exceptionmessage, exceptionsource, exceptionstacktrace, controller, action, url, httpmethod, requestjson, responsejson, companyid, innerexceptionid) VALUES (NULL, NULL, NULL, NULL, NULL, NULL, @datetime, NULL, 'ERROR', NULL, NULL, @exceptionMessage, @exceptionSource, @exceptionStackTrace, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)";

        using (var command = new NpgsqlCommand(cmdText, connection))
        {
            command.Parameters.AddWithValue("@datetime", DateTime.UtcNow);
            command.Parameters.AddWithValue("@exceptionMessage", ex.Message);
            command.Parameters.AddWithValue("@exceptionSource", ex.Source);
            command.Parameters.AddWithValue("@exceptionStackTrace", ex.StackTrace);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    Console.WriteLine(ex);
    throw;
}