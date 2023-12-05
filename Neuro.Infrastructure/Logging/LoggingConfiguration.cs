using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Neuro.Infrastructure.Logging;

public static class LoggingConfiguration
{
    public static IServiceCollection AddNeuroLogging(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            return services;
        
        var columnOptions = new ColumnOptions
        {
            TimeStamp = { ColumnName = "DateTime" },
            PrimaryKey = new SqlColumn { ColumnName = "Id", DataType = SqlDbType.Int },
            AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn { ColumnName = "Action", DataType = SqlDbType.NVarChar, DataLength = 100 },
                new SqlColumn { ColumnName = "Url", DataType = SqlDbType.NVarChar, DataLength = -1 },
                new SqlColumn { ColumnName = "SourceContext", DataType = SqlDbType.NVarChar, DataLength = -1 },
                new SqlColumn { ColumnName = "HttpMethod", DataType = SqlDbType.NVarChar, DataLength = 10 },
                new SqlColumn { ColumnName = "Type", DataType = SqlDbType.NVarChar, DataLength = 100 },
                new SqlColumn { ColumnName = "Title", DataType = SqlDbType.NVarChar, DataLength = 200 },
                new SqlColumn { ColumnName = "Detail", DataType = SqlDbType.NVarChar, DataLength = -1 },
                new SqlColumn { ColumnName = "Status", DataType = SqlDbType.Int },
                new SqlColumn { ColumnName = "Instance", DataType = SqlDbType.NVarChar, DataLength = 50 },
            }
        };

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
            .WriteTo.MSSqlServer(
                connectionString: connectionString,
                tableName: "Logs",
                schemaName: "dbo",
                columnOptions: columnOptions,
                autoCreateSqlTable: true,
                restrictedToMinimumLevel: LogEventLevel.Information
            )
            .CreateLogger();

        return services;
    }
}