using FormationOfDocuments.Models;
using Serilog;

namespace FormationOfDocuments.Services.DocumetsHandlers
{
    public class DocxFileHandler : NameFileHandler
    {
        public DocxFileHandler(string filePatch, ILogger logger) : base(filePatch, logger)
        {

        }

        public override void GetTemplateFields(List<string> templateFields)
        {
            //Необходимо открыть файл на чтение и получить имена полей для заполнения и положить их в список templateFields
        }

        public override void WriteValuesByFields(List<Item> items, string pathCreationFile)
        {

        }
    }
}
