using Microsoft.EntityFrameworkCore;
using Serilog;
using FormationOfDocuments.Models.DBContext;
using Serilog.Events;
using FormationOfDocuments.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using FormationOfDocuments.Models;

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
                        tableName: "Logs",
                        autoCreateSqlTable: true))
                .CreateLogger();

            DocumentHandler documentHandler = new DocumentHandler("C:\\Users\\Daniil\\Desktop\\TestWordTemplate.docx", logger);
            var documents = documentHandler.GetTemplateFields();

            foreach( var document in documents ) 
            {
                Console.WriteLine(document);
                Console.WriteLine("--------------");
            }

            List<BookmarkReplacement> items = new List<BookmarkReplacement>()
            {
                new BookmarkReplacement("имя", "Daniil"),
                new BookmarkReplacement("количесвто_лет", "22")           
            };

            documentHandler.WriteValuesByFields(items, "C:\\Users\\Daniil\\Desktop\\NewTestWordTemplate.docx").Wait();
       }
    }    
}