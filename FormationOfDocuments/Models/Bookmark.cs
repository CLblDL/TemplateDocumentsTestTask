using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FormationOfDocuments.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormationOfDocuments.Models
{
    public class Bookmark: IDocumentElement
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public void WriteContentFile(WordprocessingDocument doc)
        {
            // Ищем закладку по имени в новом файле
            var bookmarkStart = FindBookmark(doc.MainDocumentPart.Document.Body, Name);

            if (bookmarkStart != null)
            {
                // Очищаем содержимое закладки и добавляем новый текст
                bookmarkStart.Parent.Descendants<BookmarkEnd>()
                    .Where(b => b.Id == bookmarkStart.Id)
                    .FirstOrDefault()?.Remove();

                bookmarkStart.Parent.InsertAfter(
                    new Run(new Text(Content)),
                    bookmarkStart
                );
            }
            else
            {
                Console.WriteLine("Закладка не найдена: " + Name);
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
