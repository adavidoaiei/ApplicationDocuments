using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HtmlAgilityPack;
using iTextSharp.text.pdf;

namespace ToolPDF
{

    public partial class ToolPDF : Form
    {
        public ToolPDF()
        {
            InitializeComponent();
        }

        private void btnOpenHTML_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbInputHTML.Text = ofd.FileName;
            }
        }

        private void btnOpenPDF_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbInputPDF.Text = ofd.FileName;
            }
        }

        private void btnGeneratePDF_Click(object sender, EventArgs e)
        {
            string htmlUploaded = File.ReadAllText(tbInputHTML.Text);
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(htmlUploaded);

            HtmlNodeCollection collection = null;
            collection = document.DocumentNode.SelectNodes("//input");
            //#region "Valideaza Template"            
            Input[] listIdsHtml = new Input[collection.Count];
            int index = 0;
            if (collection != null)
            {
                foreach (HtmlNode input in collection)
                {
                    if (input.Attributes["id"].Value != null)
                    {
                        string id = input.Attributes["id"].Value;
                        string style = input.Attributes["style"].Value;
                        string type = input.Attributes["type"].Value;
                        string[] properties = style.Split(';');
                        Input inp = new Input();
                        inp.Id = id;
                        inp.Type = type;
                        foreach (string property in properties)
                        {
                            string[] values = property.Split(':');
                            if (values[0].Trim() == "left")
                                inp.Left = Convert.ToInt32(values[1].Trim().Substring(0, values[1].Trim().Length - 2));
                            if (values[0].Trim() == "bottom")
                                inp.Bottom = Convert.ToInt32(values[1].Trim().Substring(0, values[1].Trim().Length - 2));
                            if (values[0].Trim() == "width")
                                inp.Width = Convert.ToInt32(values[1].Trim().Substring(0, values[1].Trim().Length - 2));
                        }
                        listIdsHtml[index] = inp;
                        index++;
                    }
                }
            }

            string inputFile = tbInputPDF.Text;
            string outputFile = Path.GetDirectoryName(inputFile) + "\\Output_" + Path.GetFileName(tbInputPDF.Text);
            using (PdfStamper stamper = new PdfStamper(new PdfReader(inputFile), File.Create(outputFile)))
            {
                foreach (Input input in listIdsHtml)
                {
                    int leftOffset = 0;
                    int bottomOffset = 0;
                    if (input.Type == "text")
                    {
                        //TextField tf = new TextField(stamper.Writer, new iTextSharp.text.Rectangle(input.Bottom, input.Left, input.Bottom + input.Width, input.Left + 20), Guid.NewGuid().ToString());
                        TextField tf = new TextField(stamper.Writer, new iTextSharp.text.Rectangle(((input.Left + leftOffset) * 72 / 96), ((input.Bottom + bottomOffset) * 72 / 96), ((input.Left + leftOffset) * 72 / 96) + (input.Width * 72 / 96), ((input.Bottom + bottomOffset) * 72 / 96) + 20), input.Id);
                        //tf.Text = input.Id;
                        stamper.AddAnnotation(tf.GetTextField(), 1);
                    }
                    if (input.Type == "checkbox")
                    {
                        TextField tf = new TextField(stamper.Writer, new iTextSharp.text.Rectangle(((input.Left + leftOffset) * 72 / 96), ((input.Bottom + bottomOffset) * 72 / 96), ((input.Left + leftOffset) * 72 / 96) + 15, ((input.Bottom + bottomOffset) * 72 / 96) + 15), input.Id);
                        //tf.Text = "X";
                        tf.BorderStyle = 2;
                        //tf.Text = input.Id;
                        stamper.AddAnnotation(tf.GetTextField(), 1);
                    }
                }
                //TextField tf = new TextField(stamper.Writer, new iTextSharp.text.Rectangle(20, 100, 20 + 100, 100 + 10), Guid.NewGuid().ToString());
                //tf.Text = "Hello";
                //stamper.AddAnnotation(tf.GetTextField(), 1);             
                //stamper.Close(); 
            }
        }
    }
}
