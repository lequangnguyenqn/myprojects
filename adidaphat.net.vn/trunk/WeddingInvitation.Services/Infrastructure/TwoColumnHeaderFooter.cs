using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeddingInvitation.Services.Infrastructure
{
    public class TwoColumnHeaderFooter : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte _cb;

        // we will put the final number of pages in a template
        PdfTemplate _template;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont _bf;

        // This keeps track of the creation time
        DateTime _printTime = DateTime.Now;

        #region Properties

        public string Title { get; set; }

        public string HeaderLeft { get; set; }

        public string HeaderRight { get; set; }

        public Font HeaderFont { get; set; }

        public Font FooterFont { get; set; }

        #endregion

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                _printTime = DateTime.Now;
                _bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                _cb = writer.DirectContent;
                _template = _cb.CreateTemplate(50, 50);
            }
            catch (DocumentException)
            {
            }
            catch (IOException)
            {
            }
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            //Rectangle pageSize = document.PageSize;

            //if (Title != string.Empty)
            //{
            //    _cb.BeginText();
            //    _cb.SetFontAndSize(_bf, 15);
            //    _cb.SetRGBColorFill(50, 50, 200);
            //    _cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetTop(40));
            //    _cb.ShowText(Title);
            //    _cb.EndText();
            //}

            //if (HeaderLeft + HeaderRight != string.Empty)
            //{
            //    var headerTable = new PdfPTable(2);
            //    headerTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    headerTable.TotalWidth = pageSize.Width - 80;
            //    headerTable.SetWidthPercentage(new float[] { 45, 45 }, pageSize);

            //    var headerLeftCell = new PdfPCell(new Phrase(8, HeaderLeft, HeaderFont)) { Padding = 5, PaddingBottom = 8, BorderWidthRight = 0 };
            //    headerTable.AddCell(headerLeftCell);

            //    var headerRightCell = new PdfPCell(new Phrase(8, HeaderRight, HeaderFont))
            //                              {
            //                                  HorizontalAlignment = Element.ALIGN_RIGHT,
            //                                  Padding = 5,
            //                                  PaddingBottom = 8,
            //                                  BorderWidthLeft = 0
            //                              };
            //    headerTable.AddCell(headerRightCell);

            //    _cb.SetRGBColorFill(0, 0, 0);
            //    headerTable.WriteSelectedRows(0, -1, pageSize.GetLeft(40), pageSize.GetTop(50), _cb);
            //}
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            int pageN = writer.PageNumber;
            var text = "Trang " + pageN + "/";
            var len = _bf.GetWidthPoint(text, 8);

            Rectangle pageSize = document.PageSize;

            _cb.SetRGBColorFill(100, 100, 100);

            _cb.BeginText();
            _cb.SetFontAndSize(_bf, 8);
            _cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(30));
            _cb.ShowText(text);
            _cb.EndText();

            _cb.AddTemplate(_template, pageSize.GetLeft(40) + len, pageSize.GetBottom(30));

            //_cb.BeginText();
            //_cb.SetFontAndSize(_bf, 8);
            //_cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
            //    "Printed On " + _printTime.ToString(CultureInfo.InvariantCulture),
            //    pageSize.GetRight(40),
            //    pageSize.GetBottom(30), 0);
            //_cb.EndText();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            _template.BeginText();
            _template.SetFontAndSize(_bf, 8);
            _template.SetTextMatrix(0, 0);
            _template.ShowText("" + (writer.PageNumber - 1));
            _template.EndText();
        }

    }
}
