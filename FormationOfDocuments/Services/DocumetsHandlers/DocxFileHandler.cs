using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FormationOfDocuments.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

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
            using (WordprocessingDocument doc = WordprocessingDocument.Open(_pathTemplateFile, false))
            {
                // Получаем коллекцию закладок из основного текста документа
                var bookmarks = GetBookmarks(doc.MainDocumentPart.Document.Body);

                // Теперь у тебя есть коллекция закладок для дальнейшей обработки
                foreach (var bookmark in bookmarks)
                {
                    if (bookmark.Name != "_GoBack")
                    {
                        templateFields.Add(bookmark.Name);
                        Console.WriteLine("Закладка: " + bookmark.Name);
                    }
                }
            }
        }

        static BookmarkStart[] GetBookmarks(OpenXmlElement element)
        {
            var bookmarks = element.Elements<BookmarkStart>().ToList();

            foreach (var childElement in element.Elements())
            {
                bookmarks.AddRange(GetBookmarks(childElement));
            }

            return bookmarks.ToArray();
        }

        public override void WriteValuesByFields(List<BookmarkReplacement> items, string pathCreationFile)
        {
            // Создаем копию шаблона
            System.IO.File.Copy(_pathTemplateFile, pathCreationFile, true);

            using (WordprocessingDocument doc = WordprocessingDocument.Open(pathCreationFile, true))
            {
                foreach (var item in items)
                {
                    // Ищем закладку по имени в новом файле
                    var bookmarkStart = FindBookmark(doc.MainDocumentPart.Document.Body, item.NameFields);

                    if (bookmarkStart != null)
                    {
                        // Очищаем содержимое закладки и добавляем новый текст
                        bookmarkStart.Parent.Descendants<BookmarkEnd>()
                            .Where(b => b.Id == bookmarkStart.Id)
                            .FirstOrDefault()?.Remove();

                        bookmarkStart.Parent.InsertAfter(
                            new Run(new Text(item.Content)),
                            bookmarkStart
                        );
                    }
                    else
                    {
                        Console.WriteLine("Закладка не найдена: " + item.NameFields);
                    }
                }

                // Сохраняем файл после всех изменений
                doc.MainDocumentPart.Document.Save();
                Console.WriteLine("Новый файл создан");
            }
        }
        /// <summary>
        /// Рекурсивная функция для поиска закладки по имени
        /// </summary>
        /// <param name="element"></param>
        /// <param name="bookmarkName"></param>
        /// <returns></returns>
        static BookmarkStart FindBookmark(OpenXmlElement element, string bookmarkName)
        {
            return element.Descendants<BookmarkStart>()
                .Where(b => b.Name == bookmarkName)
                .FirstOrDefault();
        }
    }
}
