using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormationOfDocuments.interfaces
{
    public interface IDocumentElement
    {
        string Name { get; set; }
        string Content { get; set; } 

        void WriteContentFile(WordprocessingDocument doc);
    }
}
