using FormationOfDocuments.Models;
using Serilog;

namespace FormationOfDocuments.Services.DocumetsHandlers
{
    public class RtfFileHandler : NameFileHandler
    {
        public RtfFileHandler(string filePatch, ILogger logger) : base(filePatch, logger)
        {

        }

        public override void GetTemplateFields(List<string> templateFields)
        {
            //Необходимо открыть файл на чтение и получить имена полей для заполнения и положить их в список templateFields
        }

        public override void WriteValuesByFields(List<BookmarkReplacement> items, string pathCreationFile)
        {

        }
    }
}
