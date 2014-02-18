using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using HtmlAgilityPack;
using System.Collections;
using iTextSharp.text.pdf;
using FtpLib;
using System.IO;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
//using Microsoft.Office.Interop.Word;
using DataTable = System.Data.DataTable;

namespace Administrare
{
    public partial class AdminDocuments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsLogged"] == null)
            {
                Response.Write("Nu sunteti autentificat");
                Response.End();
                return;
            }
            string sql = "SELECT TemplatesDocuments.Id, TemplatesDocuments.Name, TemplatesDocuments.PathPDF, TemplatesDocuments.PathHTML, TemplatesDocuments.AddedBy, TemplatesDocuments.IdCategorie, ISNULL(TemplatesDocuments.InProd, 'false') AS InProd, TemplatesDocuments.CreationDate, Categories.Category FROM TemplatesDocuments INNER JOIN Categories ON TemplatesDocuments.IdCategorie = Categories.Id WHERE (TemplatesDocuments.IsActive = 1) ORDER BY TemplatesDocuments.CreationDate DESC";
            SqlDataSource1.SelectCommand = sql;
            grdDocuments.DataBind();
        }

        public bool ValidateInput()
        {
            string mesaj = string.Empty;
            bool isValid = true;

            if (tbNumeDocument.Text == string.Empty)
            {
                mesaj = "Completeaza Nume Document <br/>";
                isValid = false;
            }

            //if (fuImagineDocument.FileName == string.Empty)
            //{
            //    mesaj += "Introduceti cale imagine <br/>";
            //    isValid = false;
            //}

            //if (fuHtmlDocument.FileName == string.Empty)
            //{
            //    mesaj += "Introduceti HTML <br/>";
            //    isValid = false;
            //}

            //if (fuPdfDocument.FileName == string.Empty)
            //{
            //    mesaj += "Introduceti PDF <br/>";
            //    isValid = false;
            //}

            if (!isValid)
                lblError.Text = mesaj;

            return isValid;
        }

        protected void btnAdaugaDocument_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            #region "Save Files on Disk"
            string name = tbNumeDocument.Text;
            name = name.Replace(" ", "_").ToLower();
            string path = MapPath("~") + "\\Templates\\" + name;
            if (!Directory.Exists(MapPath("~") + "\\Templates\\" + name))
            {
                Directory.CreateDirectory(MapPath("~") + "\\Templates\\" + name);                
            }

            if (File.Exists(path + "\\" + name + ".png"))
                File.Delete(path + "\\" + name + ".png");
            fuImagineDocument.SaveAs(path + "\\" + name + ".png");
            if (File.Exists(path + "\\" + name + ".html"))
                File.Delete(path + "\\" + name + ".html");
            fuHtmlDocument.SaveAs(path + "\\" + name + ".html");
            string htmlUploaded = File.ReadAllText(path + "\\" + name + ".html");
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            string htmlString = htmlUploaded;
            document.LoadHtml(htmlString);
            System.Drawing.Image img = System.Drawing.Image.FromFile(path + "\\" + name + ".png");
            document.GetElementbyId("continut").Attributes["style"].Value = @"background-image:url('{0}'); width: " + img.Width + "px; height: " + img.Height + "px;";
            img.Dispose();
            string htmlForSave = document.GetElementbyId("continut").OuterHtml;
            File.WriteAllText(path + "\\" + name + ".html", htmlForSave);
            if (File.Exists(path + "\\" + name + ".pdf"))
                File.Delete(path + "\\" + name + ".pdf");
            fuPdfDocument.SaveAs(path + "\\" + name + ".pdf");
            if (File.Exists(path + "\\" + name + Path.GetExtension(fuDocDocument.FileName)))
                File.Delete(path + "\\" + name + Path.GetExtension(fuDocDocument.FileName));
            fuDocDocument.SaveAs(path + "\\" + name + Path.GetExtension(fuDocDocument.FileName));

            UploadDocument(name + ".html");
            UploadDocument(name + ".pdf");
            UploadDocument(name + ".png");
            UploadDocument(name + Path.GetExtension(fuDocDocument.FileName));

            #endregion

            htmlUploaded = File.ReadAllText(path + "\\" + name + ".html");
            document = new HtmlAgilityPack.HtmlDocument();
            htmlString = htmlUploaded;
            document.LoadHtml(htmlString);

            HtmlNodeCollection collection = null;
            collection = document.DocumentNode.SelectNodes("//input");
            #region "Valideaza Template"            
            //ArrayList listIdsHtml = new ArrayList();
            //if (collection != null)
            //{
            //    foreach (HtmlNode input in collection)
            //    {
            //        string id = input.Attributes["id"].Value;
            //        listIdsHtml.Add(id);
            //    }
            //}

            //PdfReader reader = new PdfReader(path + "\\" + name + ".pdf");            
            //PdfStamper stamper = new PdfStamper(reader, new FileStream(path + "\\_" + name + ".pdf", FileMode.OpenOrCreate));

            //AcroFields form = stamper.AcroFields;
            //var fieldKeys = form.Fields.Keys;
            //ArrayList listFieldsPDF = new ArrayList();
            //foreach (string fieldKey in fieldKeys)
            //{
            //    listFieldsPDF.Add(fieldKey);
            //}
            
            //stamper.Close();
            //reader.Close();
            //File.Delete(path + "\\_" + name + ".pdf");

            //string mesaj1 = CompareColectii(listIdsHtml, listFieldsPDF);
            //if (mesaj1 != "")
            //lblError.Text += "Campuri care exista in html dar nu exista in pdf: " + mesaj1  + "<br/>";
            //string mesaj2 = CompareColectii(listFieldsPDF, listIdsHtml);
            //if (mesaj2 != "")
            //lblError.Text += "Campuri care exista in pdf si nu exista in html: " + mesaj2;
            //if (lblError.Text != "")
            //    return;
            #endregion

            if (tbNumeDocument.ReadOnly == false) // this means insert
            {                
                #region "Insert in database"
                TemplatesDocuments template = new TemplatesDocuments();
                template.Name = tbNumeDocument.Text;
                template.IdCategorie = Convert.ToInt32(ddlCategorii.SelectedValue);
                template.CreationDate = DateTime.Now;
                template.IsActive = true;
                template.CreatedBy = Convert.ToInt32(Session["UserId"]);
                template.PathTemplate = tbNumeDocument.Text.Replace(" ", "_").ToLower();
                template.UserId = Convert.ToInt32(ddlNumeFirme.SelectedValue);
                int idTemplate = template.Save();

                DocumentsTableAdapters.TemplateFieldsTableAdapter adapter = new DocumentsTableAdapters.TemplateFieldsTableAdapter();
                Documents.TemplateFieldsDataTable table = new Documents.TemplateFieldsDataTable();
                if (collection != null)
                {
                    foreach (HtmlNode input in collection)
                    {
                        string idInput = input.Attributes["id"].Value;
                        Documents.TemplateFieldsRow row = table.NewTemplateFieldsRow();
                        row.TemplateId = idTemplate;
                        row.Field = idInput;
                        table.AddTemplateFieldsRow(row);
                    }
                }
                adapter.Update(table);
                #endregion

                #region "reset fields"
                tbNumeDocument.Text = "";
                ddlCategorii.SelectedIndex = 0;
                #endregion
            }
            else if (tbNumeDocument.ReadOnly == true) // this means update
            {
                //do nothing only the files will be updated 
            }

            #region "Genereaza DropDownListDocumente"
            //string html = GenerateDropDownListDocumente();
            //File.WriteAllText(Server.MapPath("~") + "\\Helpers\\DropDownDocumente.html", html);
            //SaveDropDownListDocumenteHTMLOnFTP();
            #endregion

            grdDocuments.DataBind();
        }

        public string CompareColectii(ArrayList colectie1, ArrayList colectie2)
        {
            bool exista = false;
            string ret = string.Empty;
            foreach (string item1 in colectie1)
            {
                exista = false;
                foreach (string item2 in colectie2)
                {
                    if (item1 == item2)
                    {
                        exista = true;
                        break;
                    }
                }
                if (exista == false)
                    ret += item1 + ", ";
            }

            return ret;
        }

        //protected void SendMail(string name)
        //{
        //    MailMessage message = new MailMessage();
        //    message.From = new MailAddress("vasile.bularca1984@gmail.com");// Gmail Address from where you send the mail
        //    message.To.Add("negoiescu.mihai@gmail.com");
        //    const string fromPassword = "dumitru1984";//Password of your gmail address
        //    message.Subject = name;
        //    message.Body = name;
        //    string png = MapPath("~") + "\\Templates\\" + Path.GetFileNameWithoutExtension(name) + "\\" + name + ".png";
        //    if (File.Exists(png))
        //        message.Attachments.Add(new Attachment(png));            
        //    string pdf = MapPath("~") + "\\Templates\\" + Path.GetFileNameWithoutExtension(name) + "\\" + name + ".pdf";
        //    if (File.Exists(pdf))
        //        message.Attachments.Add(new Attachment(pdf));
        //    string doc = MapPath("~") + "\\Templates\\" + Path.GetFileNameWithoutExtension(name) + "\\" + name + ".doc";
        //    if (File.Exists(doc))
        //        message.Attachments.Add(new Attachment(doc));
        //    var smtp = new System.Net.Mail.SmtpClient();
        //    {
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.Port = 587;
        //        smtp.EnableSsl = true;
        //        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //        smtp.Credentials = new NetworkCredential(message.To[0].Address, fromPassword);
        //        smtp.Timeout = 20000;
        //    }
        //    smtp.Send(message);
        //}
        
        /// <summary>
        /// Method to upload the specified file to the specified FTP Server
        /// http://www.codeproject.com/Articles/17202/Simple-FTP-demo-application-using-C-Net-2-0
        /// </summary>
        /// <param name="filename">file full name to be uploaded</param>
        string ftpServerIP = "ftp.edocumente.com";
        string ftpFolder = "";
        string ftpUserID = "cornel.adavidoaieix";
        string ftpPassword = "Dumitru-1984";
        private void UploadDocument(string filename)
        {
            if (Request.Url.AbsoluteUri.Contains("localhost"))
            {
            //    if (filename.Contains(".png") || filename.Contains(".pdf") || filename.Contains(".html"))
            //    {
            //        string pathLocal = @"C:\Users\dadavidoaiei\Desktop\Files\Documents\Acte\Templates\";
            //        string directory = Path.GetFileNameWithoutExtension(filename);
            //        if (!Directory.Exists(pathLocal + "\\" + directory))
            //            Directory.CreateDirectory(pathLocal + "\\" + directory);
            //        string sourcePath = MapPath("~") + "\\Templates\\" + Path.GetFileNameWithoutExtension(filename) + "\\" + filename;
            //        if (File.Exists(pathLocal + "\\" + directory + "\\" + filename))
            //            File.Delete(pathLocal + "\\" + directory + "\\" + filename);
            //        File.Copy(sourcePath, pathLocal + "\\" + directory + "\\" + filename);
            //    }
            }
            else
            {
                using (FtpConnection ftp = new FtpConnection(ftpServerIP, ftpUserID, ftpPassword))
                {

                    ftp.Open(); /* Open the FTP connection */
                    ftp.Login(); /* Login using previously provided credentials */

                    //do some processing

                    try
                    {
                        string path = MapPath("~") + "\\Templates\\" + Path.GetFileNameWithoutExtension(filename) + "\\" + filename;
                        ftp.SetCurrentDirectory("/cornel.adavidoaieix/Templates");
                        if (!ftp.DirectoryExists("/cornel.adavidoaieix/Templates/" + Path.GetFileNameWithoutExtension(filename)))
                            ftp.CreateDirectory(Path.GetFileNameWithoutExtension(filename));
                        ftp.SetCurrentDirectory("/cornel.adavidoaieix/Templates/" + Path.GetFileNameWithoutExtension(filename));
                        if (ftp.FileExists(filename))
                            ftp.RemoveFile(filename);
                        ftp.PutFile(path, filename); /* upload c:\localfile.txt to the current ftp directory as file.txt */
                    }
                    catch (FtpException e)
                    {
                        throw e;
                    }
                }
            }
        }

        public void DeleteDirectoryFtp(string name) 
        {
            using (FtpConnection ftp = new FtpConnection(ftpServerIP, ftpUserID, ftpPassword))
            {

                ftp.Open(); /* Open the FTP connection */
                ftp.Login(); /* Login using previously provided credentials */

                //do some processing

                try
                {
                    if (ftp.DirectoryExists("/cornel.adavidoaieix/Templates/" + name))
                    {
                        ftp.SetCurrentDirectory("/cornel.adavidoaieix/Templates/" + name);
                        FtpFileInfo[] files = ftp.GetFiles();
                        foreach (FtpFileInfo file in files)
                        {
                            ftp.RemoveFile("/cornel.adavidoaieix/Templates/" + name + "/" + file.Name);
                        }
                        ftp.RemoveDirectory("/cornel.adavidoaieix/Templates/" + name);
                    }
                }
                catch (FtpException e)
                {
                    throw e;
                }
            }
        }

        public void CreateDirectoryOnFTP(string filename)
        {
            string path = "ftp://" + ftpServerIP + "/Templates/" + Path.GetFileNameWithoutExtension(filename);
            FtpWebRequest createFolderFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
            createFolderFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            createFolderFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            //createFolderFTP.UsePassive = true;
            createFolderFTP.UseBinary = true;
            //createFolderFTP.KeepAlive = false;
            var resp = (FtpWebResponse)createFolderFTP.GetResponse();
            resp.Close();
        }

        protected void lbEdit_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            DocumentsTableAdapters.TemplatesDocumentsTableAdapter adapter = new DocumentsTableAdapters.TemplatesDocumentsTableAdapter();
            Documents.TemplatesDocumentsRow row = (Documents.TemplatesDocumentsRow)adapter.GetDataById(id).Rows[0];

            tbNumeDocument.Text = row.Name;
            if (!row.IsIdCategorieNull())
                ddlCategorii.SelectedValue = row.IdCategorie.ToString();
            tbNumeDocument.ReadOnly = true;
            tbNumeDocument.BackColor = System.Drawing.Color.Gray;
            btnAdaugaDocument.Text = "Update Document";
        }

        protected void lbAdaugaDocument_Click(object sender, EventArgs e)
        {
            tbNumeDocument.BackColor = System.Drawing.Color.White;
            tbNumeDocument.ReadOnly = false;
            ddlCategorii.SelectedIndex = 0;
            tbNumeDocument.Text = string.Empty;
            btnAdaugaDocument.Text = "Adauga Document"; 
        }

        protected void lbSterge_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            DocumentsTableAdapters.TemplatesDocumentsTableAdapter adapter = new DocumentsTableAdapters.TemplatesDocumentsTableAdapter();
            Documents.TemplatesDocumentsRow row = (Documents.TemplatesDocumentsRow)adapter.GetDataById(id).Rows[0];
            if (Directory.Exists(Server.MapPath("~") + "\\Templates\\" + row.PathTemplate))
                Directory.Delete(Server.MapPath("~") + "\\Templates\\" + row.PathTemplate, true);   
            DeleteDirectoryFtp(row.PathTemplate);
            adapter.Delete(id);
            SqlCommand command = new SqlCommand();
            command.CommandText = "DELETE FROM TemplateFields WHERE TemplateId = " + id;
            command.Connection = new SqlConnection();
            command.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
            grdDocuments.DataBind();
        }

        protected void ddlCategorie_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT Documents.Id, Documents.Name, Documents.PathPDF, Documents.PathHTML, Documents.AddedBy, Documents.IdCategorie, Documents.IsActive, Documents.CreationDate, Categories.Category FROM TemplatesDocuments INNER JOIN Categories ON Documents.IdCategorie = Categories.Id WHERE (Documents.IsActive = 1) {0} ORDER BY Documents.CreationDate DESC";

            if (ddlFiltruCategorie.SelectedIndex > 0)
                sql = string.Format(sql, " AND (IdCategorie = " + ddlFiltruCategorie.SelectedValue + ")");
            else
                sql = string.Format(sql, "");
            
            SqlDataSource1.SelectCommand = sql;
            
            grdDocuments.DataBind();
        }

        public string GenerateDropDownListDocumente()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<select onchange=\"change()\" id=\"select\">" + System.Environment.NewLine);

            string sql = "SELECT Id, Category FROM Categories";
            DataTable categories = GetDataTable(sql);
            foreach(DataRow rowCategorie in categories.Rows) 
            {
                sb.Append("<optgroup label=\"" + rowCategorie["Category"].ToString() +  "\">" + System.Environment.NewLine);
                string sqlDoc = "SELECT Id, Name FROM TemplatesDocuments WHERE IdCategorie = '" + rowCategorie["Id"].ToString() + "'";
                DataTable documents = GetDataTable(sqlDoc);
                foreach (DataRow drDocument in documents.Rows){
                    sb.Append("<option value=\"" + drDocument["Name"].ToString().Replace(" ", "_").ToLower() + "\">" + drDocument["Name"].ToString() + "</option>");                        
                }
                sb.Append("</optgroup>" + System.Environment.NewLine); 
            }
            
            //<optgroup label="Acte Auto">
            //    <option value="cerere-inmatriculare">Cerere de inmatriculare</option>
            //</optgroup>
            //<optgroup label="Contabilitate">
            //    <option>Factura, in curand</option>
            //    <option>Chitanta, in curand</option>
            //</optgroup>
            //<optgroup label="Acte Angajat">
            //    <option>Adeverinta de angajat, in curand</option>
            //    <option>Adeverinta de venit, in curand</option>
            //</optgroup>
            sb.Append("</select>");

            return sb.ToString();
        }

        public DataTable GetDataTable(string sql) {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand();
            adapter.SelectCommand.CommandText = sql;
            adapter.SelectCommand.Connection = new SqlConnection();
            adapter.SelectCommand.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds.Tables[0];
        }

        protected void Id_Click(object sender, EventArgs e)
        {
            #region "Genereaza DropDownListDocumente"
            //string html = GenerateDropDownListDocumente();
            //File.WriteAllText(Server.MapPath("~") + "\\Helpers\\DropDownDocumente.html", html);
            //SaveDropDownListDocumenteHTMLOnFTP();
            #endregion
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                throw new Exception("bla");               
            }
            catch (Exception ex)
            {
                DocumentsTableAdapters.LogErrorTableAdapter adapter = new DocumentsTableAdapters.LogErrorTableAdapter();
                Documents.LogErrorDataTable table = new Documents.LogErrorDataTable();
                Documents.LogErrorRow row = table.NewLogErrorRow();
                row.Message = ex.Message;
                row.StackTrace = ex.StackTrace;
                table.AddLogErrorRow(row);
                adapter.Update(table);
            }
        }

        protected void lbGoProd_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);            
            DocumentsTableAdapters.TemplatesDocumentsTableAdapter adapter = new DocumentsTableAdapters.TemplatesDocumentsTableAdapter();
            Documents.TemplatesDocumentsRow row = (Documents.TemplatesDocumentsRow)adapter.GetDataById(id).Rows[0];
            row.InProd = true;
            adapter.Update(row);
            CopyFolderToProd(row.PathTemplate);
        }

        public void CopyFolderToProd(string name)
        {
            using (FtpConnection ftp = new FtpConnection(ftpServerIP, "cornel.adavidoaiei", ftpPassword))
            {

                ftp.Open(); /* Open the FTP connection */
                ftp.Login(); /* Login using previously provided credentials */

                //do some processing

                try
                {
                    string path = MapPath("~") + "\\Templates\\" + name;                    
                    if (!ftp.DirectoryExists("/cornel.adavidoaiei/Templates/" + name))
                        ftp.CreateDirectory("/cornel.adavidoaiei/Templates/" + name);
                    ftp.SetCurrentDirectory("/cornel.adavidoaiei/Templates/" + name);
                    string[] files = Directory.GetFiles(path);
                    foreach (string file in files)
                        ftp.PutFile(file);
                }
                catch (FtpException e)
                {
                    throw e;
                }
            }
        }

        //public static void Word2PDF(string fileName)
        //{
        //    Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
        //    Microsoft.Office.Interop.Word.Document wordDocument = null;
        //    object paramSourceDocPath = fileName + @".doc";
        //    object paramMissing = Type.Missing;
        //    string paramExportFilePath = fileName + @".pdf";
        //    WdExportFormat paramExportFormat = WdExportFormat.wdExportFormatPDF;
        //    bool paramOpenAfterExport = false;

        //    WdExportOptimizeFor paramExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint;

        //    WdExportRange paramExportRange = WdExportRange.wdExportAllDocument;
        //    int paramStartPage = 0;

        //    int paramEndPage = 0;
        //    WdExportItem paramExportItem = WdExportItem.wdExportDocumentContent;

        //    bool paramIncludeDocProps = true;
        //    bool paramKeepIRM = true;

        //    WdExportCreateBookmarks paramCreateBookmarks =
        //    WdExportCreateBookmarks.wdExportCreateWordBookmarks;

        //    bool paramDocStructureTags = true;
        //    bool paramBitmapMissingFonts = true;

        //    bool paramUseISO19005_1 = false;

        //    try
        //    {
        //        // Open the source document.
        //        wordDocument = wordApplication.Documents.Open(
        //        ref paramSourceDocPath, ref paramMissing, ref paramMissing,
        //        ref paramMissing, ref paramMissing, ref paramMissing,
        //        ref paramMissing, ref paramMissing, ref paramMissing,
        //        ref paramMissing, ref paramMissing, ref paramMissing,
        //        ref paramMissing, ref paramMissing, ref paramMissing,
        //        ref paramMissing);

        //        // Export it in the specified format.
        //        if (wordDocument != null)
        //            wordDocument.ExportAsFixedFormat(paramExportFilePath,
        //            paramExportFormat, paramOpenAfterExport,
        //            paramExportOptimizeFor, paramExportRange, paramStartPage,
        //            paramEndPage, paramExportItem, paramIncludeDocProps,
        //            paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
        //            paramBitmapMissingFonts, paramUseISO19005_1,
        //            ref paramMissing);
        //    }
        //    catch (Exception ex)
        //    {

        //        // Respond to the error
        //    }
        //    finally
        //    {
        //        // Close and release the Document object.
        //        if (wordDocument != null)
        //        {
        //            wordDocument.Close(ref paramMissing, ref paramMissing,
        //            ref paramMissing);
        //            wordDocument = null;
        //        }
        //        // Quit Word and release the ApplicationClass object.
        //        if (wordApplication != null)
        //        {
        //            wordApplication.Quit(ref paramMissing, ref paramMissing,
        //            ref paramMissing);
        //            wordApplication = null;
        //        }
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //    }
        //}

    }
}