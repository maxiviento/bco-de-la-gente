using iTextSharp.text;
using iTextSharp.text.pdf;
using System;

namespace Core.CiDi.Documentos.Utils
{
    public class ITextEvents : PdfPageEventHelper
    {
        DateTime PrintTime = DateTime.Now;

        #region Fields
        private string _header;
        public string path;
        public bool footer = true;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion

        PdfTemplate total;

        public override void OnStartPage(PdfWriter writer, iTextSharp.text.Document document)
        {
            //PdfContentByte cbWaterMark = writer.DirectContentUnder;
            //Image watermark = Image.GetInstance(path + "\\MarcaDeAgua.png");
            //watermark.SetAbsolutePosition(50, 300);
            //watermark.ScaleToFit(500f, 500f);

            //document.Add(watermark);

            //base.OnStartPage(writer, document);
        }
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            total = writer.DirectContent.CreateTemplate(100, 100);
            total.BoundingBox = new iTextSharp.text.Rectangle(-20, -20, 100, 100);
        }
        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            Image watermark;

            // Crea la imagen de marca de agua
            watermark = Image.GetInstance(path + "\\MarcaDeAgua.png");
            // Cambia el tamaño de la imagen
            watermark.ScaleToFit(500f, 500f);
            // Se indica que la imagen debe almacenarse como fondo
            watermark.Alignment = iTextSharp.text.Image.UNDERLYING;
            // Coloca la imagen en una posición absoluta
            watermark.SetAbsolutePosition(50, 300);

            // Imprime la imagen como fondo de página
            doc.Add(watermark);

            base.OnStartPage(writer, doc);

            if (footer)
            {
                //Footer Image
                Image imgFooterLogoCordoba = Image.GetInstance(path + "\\LogoGobCba_2016_Footer.png");

                imgFooterLogoCordoba.SetAbsolutePosition(200, 0);
                imgFooterLogoCordoba.ScaleToFit(200f, 200f);

                PdfContentByte cbFoot = writer.DirectContent;
                PdfTemplate tp = cbFoot.CreateTemplate(imgFooterLogoCordoba.Width, imgFooterLogoCordoba.Height);
                tp.AddImage(imgFooterLogoCordoba);

                cbFoot.AddTemplate(tp, 0, 8);

                //PdfContentByte cb = writer.DirectContent;
                //ColumnText ct = new ColumnText(cb);

                //cb.BeginText();
                //cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10.0f);
                //cb.SetTextMatrix(doc.RightMargin, doc.BottomMargin);
                //cb.ShowText(String.Format("{0} {1}", string.Concat(DateTime.Now.ToShortDateString(), " | "), " Página " + doc.PageNumber.ToString()));
                //cb.EndText();
            }
        }
    }
}