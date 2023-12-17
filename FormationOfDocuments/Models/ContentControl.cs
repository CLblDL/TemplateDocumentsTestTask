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
    public class ContentControl: IDocumentElement
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public void WriteContentFile(WordprocessingDocument doc)
        {
            SdtElement textBox = GetSdtElementById(doc, Name);

            if (textBox is SdtBlock block)
            {
                // Очищаем текущий текст
                block.Descendants<Text>().ToList().ForEach(t => t.Text = "");

                // Добавляем новый текст
                Text newText = new Text(Content);
                block.Descendants<Run>().First().AppendChild(newText);
            }
        }

        static SdtElement GetSdtElementById(WordprocessingDocument doc, string controlId)
        {
            var sdtElements = doc.MainDocumentPart.Document.Descendants<SdtElement>();
            return sdtElements.FirstOrDefault(sdt => sdt.SdtProperties.GetFirstChild<Tag>()?.Val == controlId);
        }
    }
}
