using Microsoft.EntityFrameworkCore;
using Serilog;
using FormationOfDocuments.Models.DBContext;
using Serilog.Events;

namespace FormationOfDocuments
{
    internal class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            using var dbContext = new LogDBContext();
            // Логер подлкючается к таблице куда надо писать логи, не успел до конца разобраться с этой библитотекой 
            var logger = Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Information)
                    .WriteTo.MSSqlServer(
                        connectionString: dbContext.Database.GetDbConnection().ConnectionString,
                        tableName: "YourLogsTable",
                        autoCreateSqlTable: true))
                .CreateLogger();
       }
    }    
}