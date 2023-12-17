using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormationOfDocuments.interfaces
{
    //Нужен более просто интерфейс, метод WriteContentFile в параметре содержит класс необходимый для Word. скорее всего более верное название IWordDocumentElement
    public interface IDocumentElement
    {
        string Name { get; set; }
        string Content { get; set; } 

        /// <summary>
        /// Запись значений по элементу шаблона
        /// </summary>
        /// <param name="doc"></param>
        void WriteContentFile(WordprocessingDocument doc);
    }
}
