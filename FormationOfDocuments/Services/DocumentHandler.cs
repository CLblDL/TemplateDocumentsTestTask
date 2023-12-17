using System.Net.Mail;
using Serilog;
using FormationOfDocuments.Models;
using FormationOfDocuments.Services.email;
using FormationOfDocuments.Services.DocumetsHandlers;
using FormationOfDocuments.interfaces;

namespace FormationOfDocuments.Services
{
    public class DocumentHandler
    {
        private string _pathTemplateFile;
        private string _fileExtension;
        private List<IDocumentElement> _templateFields = new();
        private NameFileHandler _nameFileHandler;
        private readonly ILogger _logger;

        // Предпологается что при выборе файла шаблона через интерфейс программы, будет получен путь к этому шаблону
        public DocumentHandler(string pathTemplateFile, ILogger logger)
        {
            _pathTemplateFile = pathTemplateFile;
            _logger = logger;
        }

        /// <summary>
        /// Возврашает список полей шаблоны для заполнения
        /// </summary>
        // Возршаение списка полей предпологает что с помошью него можно будет отрисовать интерфейс для заполнения полей шаблона 
        public List<IDocumentElement> GetTemplateFields()
        {
            _fileExtension = Path.GetExtension(_pathTemplateFile);
            _logger.Information(string.Format("Пользователь выбрал шаблон формата {0}", _fileExtension));
            try
            {
                //От конструкции switch case можно избавиться, созданием Dictionary, который будет определяться в конструкторе класса, где ключом будет _fileExtension,
                //а значением Func, значение будет определять метод,
                //в котором будет создаваться необходимый экзмепляр обработчика формата, но от этого может ухудшиться чтение кода 
                switch (_fileExtension)
                {
                    case ".docx":
                        _nameFileHandler = new DocxFileHandler(_pathTemplateFile, _logger);
                        _nameFileHandler.GetTemplateFields(_templateFields);
                        break;
                    case ".rtf":
                        _nameFileHandler = new RtfFileHandler(_pathTemplateFile, _logger);
                        _nameFileHandler.GetTemplateFields(_templateFields);
                        break;
                    default:
                        _logger.Error(string.Format("Пользователь попытался открыть файл не поддерживаемого фомрата {0}", _fileExtension)); 
                        //Дублирование ифнормации о формате файла при выборе не поддерживаемого фомрата
                        throw new ArgumentException("Формат файла не поддерживается");
                }
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(string.Format("Файл, заданный параметром path, не найден. Ошибка {0}", ex.Message));
                //Файл, заданный параметром path, не найден.
                throw;
            }
            catch (IOException ex)
            {
                _logger.Error(string.Format("При открытии файла произошла ошибка ввода-вывода. Ошибка {0}", ex.Message));
                //При открытии файла произошла ошибка ввода-вывода.
                //Чаше всего когда файл открыт, надо предложить пользователю закрыть файлы и поробовать снова
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error("Возникло исклчюение " + ex.Message);
                throw;
            }

            return _templateFields;
        }
        
        /// <summary>
        /// Создание файла по шаблону
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pathCreationFile"></param>
        public async Task WriteValuesByFields(List<IDocumentElement> items, string pathCreationFile)
        {
            _nameFileHandler.WriteValuesByFields(items, pathCreationFile);
            _logger.Information("На основе щаблона был создан файл");
        }

        /// <summary>
        /// Отправка документа по emil
        /// </summary>
        /// <param name="items"></param>
        /// <param name="sender"></param>
        /// <param name="recipient"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task SendByMail(List<IDocumentElement> items, string sender, string password, string recipient, string subject, string body)
        {
            // Не успел посмотреть отличается ли отрпавка писем на разлчиные почтовые службы 
            _logger.Information("Попытка отправить документ по почте");
            string defoltPath = "";
            _nameFileHandler.WriteValuesByFields(items, defoltPath);

            EmailMessageConstructor emailMessageConstructor = new EmailMessageConstructor(sender,password, recipient, subject, body, defoltPath, _logger);
            _logger.Information("Пользователь отправил документ по почте");
        }
    }
}
