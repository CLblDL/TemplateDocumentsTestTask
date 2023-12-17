using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormationOfDocuments.Models
{
    /// <summary>
    /// Содержит элемент заполненого поля
    /// </summary>
    public class BookmarkReplacement
    {
        public BookmarkReplacement(string nameFields, string content)
        {
            NameFields = nameFields;
            Content = content;
        }

        public string NameFields { get; }
        public string Content { get; }
    }
}
