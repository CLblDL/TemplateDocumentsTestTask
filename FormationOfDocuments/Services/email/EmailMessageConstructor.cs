using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Serilog;
using System.Text;
using System.Threading.Tasks;

namespace FormationOfDocuments.Services.email
{
    /// <summary>
    /// Класс для работы с email
    /// </summary>
    public class EmailMessageConstructor
    {
        private readonly MailAddress _sender;
        private readonly MailAddress _recipient;
        private readonly string _subject;
        private readonly string _body;
        private readonly string _pathFile;
        private readonly ILogger _logger;

        public EmailMessageConstructor(MailAddress sender, MailAddress recipient, string subject, string body, string pathFile, ILogger logger)
        {
            _sender = sender;
            _recipient = recipient;
            _subject = subject;
            _body = body;
            _pathFile = pathFile;
            _logger = logger;
        }
    }
}
