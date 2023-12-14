using Serilog.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormationOfDocuments.Models.DBContext
{
    [Table("Logs")]
    public class LogEvent
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public LogEventLevel Level { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
