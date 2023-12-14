using FormationOfDocuments.Models;
using Serilog;

namespace FormationOfDocuments.Services.DocumetsHandlers
{
    /// <summary>
    /// Абстрактный класс от которого должны наследоваться классы обработчики различных форматов
    /// </summary>
    // Я так и не смог придумать адекватное название для этого класса
    public abstract class NameFileHandler
    {
        protected string _filepathTemplateFile;
        private readonly ILogger _logger;

        public NameFileHandler(string filepathTemplateFile, ILogger logger)
        {
            _filepathTemplateFile = filepathTemplateFile;
            _logger = logger;
        }

        /// <summary>
        /// Добавление в список, полей для заполнения
        /// </summary>
        /// <param name="templateFields"></param>
        public abstract void GetTemplateFields(List<string> templateFields);

        /// <summary>
        /// Запись значений по полям в шаблон файла
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pathCreationFile"></param>
        public abstract void WriteValuesByFields(List<Item> items, string pathCreationFile);
    }
}
