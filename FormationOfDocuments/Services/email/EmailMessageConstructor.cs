using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Serilog;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net;
using System.Xml.Linq;

namespace FormationOfDocuments.Services.email
{
    /// <summary>
    /// Класс для работы с email
    /// </summary>
    public class EmailMessageConstructor
    {
        private readonly string _sender;
        private readonly string _password;
        private readonly string _recipient;
        private readonly string _subject;
        private readonly string _body;
        private readonly string _pathFile;
        private readonly ILogger _logger;

        public EmailMessageConstructor(string sender, string password, string recipient, string subject, string body, string pathFile, ILogger logger)
        {
            _sender = sender;
            _password = password;
            _recipient = recipient;
            _subject = subject;
            _body = body;
            _pathFile = pathFile;
            _logger = logger;
        }

        public void SendEmail()
        {
            using (MailMessage mail = new MailMessage(_sender, _recipient, _subject, _body))
            {
                Attachment attachment = new Attachment(_pathFile);
                mail.Attachments.Add(attachment);

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new NetworkCredential(_sender, _password);
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        _logger.Information("Письмо успешно отправлено.");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Ошибка при отправке письма: {ex.Message}");
                    }
                }
            }
        }
    }
}
