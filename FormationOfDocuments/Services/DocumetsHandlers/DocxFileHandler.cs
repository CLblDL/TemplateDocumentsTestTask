using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FormationOfDocuments.interfaces;
using FormationOfDocuments.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace FormationOfDocuments.Services.DocumetsHandlers
{
    public class DocxFileHandler : NameFileHandler
    {
        public DocxFileHandler(string filePatch, ILogger logger) : base(filePatch, logger)
        {

        }

        public override void GetTemplateFields(List<IDocumentElement> templateFields)
        {
            //Необходимо открыть файл на чтение и получить имена полей для заполнения и положить их в список templateFields
            using (WordprocessingDocument doc = WordprocessingDocument.Open(_pathTemplateFile, false))
            {
                // Получаем коллекцию закладок из основного текста документа
                var bookmarks = GetBookmarks(doc.MainDocumentPart.Document.Body);

                foreach (var bookmark in bookmarks)
                {
                    if (bookmark.Name != "_GoBack")
                    {
                        Bookmark bookmark1 = new Bookmark() 
                        {
                            Name = bookmark.Name
                        };

                        templateFields.Add(bookmark1);
                        Console.WriteLine("Закладка: " + bookmark1.Name);
                    }
                }

                //Помимо закладок необходимо получить "элементы управления содержимым" 

                List<string> controlIds = GetAllSdtBlockIds(doc);

                Console.WriteLine("Идентификаторы текстовых полей формы:");
                foreach (var controlId in controlIds)
                {
                    ContentControl contentControl = new ContentControl()
                    {
                        Name = controlId
                    };
                    templateFields.Add(contentControl);
                    Console.WriteLine(controlId);
                }
            }
        }

        static List<string> GetAllSdtBlockIds(WordprocessingDocument doc)
        {
            var sdtElements = doc.MainDocumentPart.Document.Descendants<SdtBlock>();
            List<string> controlIds = new List<string>();

            foreach (var sdtBlock in sdtElements)
            {
                var tag = sdtBlock.Descendants<Tag>().FirstOrDefault();
                if (tag != null)
                {
                    controlIds.Add(tag.Val);
                }
            }

            return controlIds;
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

        public override void WriteValuesByFields(List<IDocumentElement> items, string pathCreationFile)
        {
            // Создаем копию шаблона
            System.IO.File.Copy(_pathTemplateFile, pathCreationFile, true);

            using (WordprocessingDocument doc = WordprocessingDocument.Open(pathCreationFile, true))
            {
                foreach (var item in items)
                {
                    item.WriteContentFile(doc);
                }

                doc.MainDocumentPart.Document.Save();
                Console.WriteLine("Новый файл создан");
            }
        }
    }
}
