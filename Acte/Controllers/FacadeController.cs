using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActeAuto.Models;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System.IO;
using System.Security.Principal;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Linq;
using ComputerBeacon.Json;
using System.Web.Security;
using System.Web.Helpers;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.Office.Interop.Word;


namespace ActeAuto.Controllers
{
    public class FacadeController : Controller
    {
        public ActionResult Completeaza()
        {            
            //if (Request.Url.AbsoluteUri.Contains("localhost") == true)
            //{
            //    FormsAuthentication.SetAuthCookie("admin", false);
            //}
            try
            {
                return View();
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
                return View();
            }                      
        }

        [HttpPost]
        public string Login(User user)
        {
            string tip = "invalid";
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.UserName, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    tip = "valid";
                }
                else
                {
                    tip = "invalid";
                }
            }
            return tip;
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Completeaza", "Facade");
        }

        [HttpPost]
        public MvcHtmlString Salveaza(FormCollection collection)
        {
            try
            {
                //throw new Exception("bla");
                if (Request.IsAuthenticated)
                {
                    JsonObject json = new JsonObject(collection[0]);
                    string document = json["document"].ToString();
                    string name = json["name_document"].ToString();
                    json.Remove("document");
                    json.Remove("name_document");

                    string file = document + " " + Guid.NewGuid() + ".pdf";

                    DocumentsTableAdapters.TemplatesDocumentsTableAdapter adapterTemplates = new DocumentsTableAdapters.TemplatesDocumentsTableAdapter();
                    IssuedDocuments doc = new IssuedDocuments();
                    doc.GuidName = file;
                    doc.Name = document;
                    doc.TemplateDocumentId = Convert.ToInt32(document);
                    doc.NameGivenByClient = name;
                    doc.UserId = Convert.ToInt32(Session["UserId"]);
                    int? lastid = (int?)doc.Save();

                    DocumentsTableAdapters.IssuedDocumentsDataTableAdapter adapterDataDocument = new DocumentsTableAdapters.IssuedDocumentsDataTableAdapter();
                    Documents.IssuedDocumentsDataDataTable tableDataDocument = new Documents.IssuedDocumentsDataDataTable();
                    foreach (string key in json.Keys)
                    {
                        Documents.IssuedDocumentsDataRow rowDataDocument = tableDataDocument.NewIssuedDocumentsDataRow();
                        rowDataDocument.Field = key;
                        rowDataDocument.Value = json[key].ToString();
                        rowDataDocument.IdDocument = lastid.Value;
                        tableDataDocument.AddIssuedDocumentsDataRow(rowDataDocument);
                        adapterDataDocument.Update(tableDataDocument);
                    }

                    StringBuilder sb = new StringBuilder();

                    DocumentsTableAdapters.IssuedDocumentsTableAdapter adapter = new DocumentsTableAdapters.IssuedDocumentsTableAdapter();
                    Documents.IssuedDocumentsDataTable table = adapter.GetDataByUserId(Convert.ToInt32(Session["UserId"]), Convert.ToInt32(document));

                    foreach (Documents.IssuedDocumentsRow row in table.Rows)
                    {
                        sb.Append("<div><a title=\"Click pe nume pentru editare document\" style=\"background-color: #f5f5f5; width: 150px !important; display:block;\" onclick=\"edit(\'" + row.Id + "\', this);\" href=\"#\">Editeaza " + row.NameGivenByClient + "</a></div>");
                    }

                    return MvcHtmlString.Create(sb.ToString());
                }
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
            return MvcHtmlString.Empty;
        }

        [HttpPost]
        public MvcHtmlString LoadDocuments(int id)
        {
            StringBuilder sb = new StringBuilder();

            DocumentsTableAdapters.IssuedDocumentsTableAdapter adapter = new DocumentsTableAdapters.IssuedDocumentsTableAdapter();
            Documents.IssuedDocumentsDataTable table = adapter.GetDataByUserId(Convert.ToInt32(Session["UserId"]), id);

            foreach (Documents.IssuedDocumentsRow row in table.Rows)
            {
                sb.Append("<div><a title=\"Click pe nume pentru editare document\" style=\"background-color: #f5f5f5; width: 150px !important; display:block;\" onclick=\"edit(\'" + row.Id + "\', this);\" href=\"#\">Editeaza " + row.NameGivenByClient + "</a></div>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        [HttpPost]
        public MvcHtmlString SearchDocument(FormCollection collection)
        {
            if (Request.IsAuthenticated)
            {
                JsonObject json = new JsonObject(collection[0]);
                
                string id_template = json["id_template"].ToString();
                string camp = json["field"].ToString();
                string valoare = json["value"].ToString();

                SqlParameter value = new SqlParameter("@Value", valoare.ToUpper());

                string sql = @"SELECT        
                                    IssuedDocuments.Id, NameGivenByClient
                                FROM            
                                    IssuedDocumentsData INNER JOIN
                                        IssuedDocuments ON IssuedDocumentsData.IdDocument = IssuedDocuments.Id
                                WHERE
                                    UserId = @UserId AND
                                    TemplateDocumentId = @TemplateDocumentId AND
                                    Field = @Field "; 
                                
                if (valoare != string.Empty) sql += " AND UPPER(Value) LIKE '%' + @Value + '%'";

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand();
                adapter.SelectCommand.CommandText = sql;
                adapter.SelectCommand.Parameters.Add("@UserId", Convert.ToInt32(Session["UserId"]));
                adapter.SelectCommand.Parameters.Add("@TemplateDocumentId", id_template);
                adapter.SelectCommand.Parameters.Add("@Field", camp);
                if (valoare != string.Empty)
                    adapter.SelectCommand.Parameters.Add("@Value", valoare);
                adapter.SelectCommand.Connection = new SqlConnection();
                adapter.SelectCommand.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                DataSet ds = new DataSet();
                adapter.Fill(ds);


                StringBuilder sb = new StringBuilder();
                
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<div><a title=\"Click pe nume pentru editare document\" style=\"background-color: #f5f5f5; width: 150px !important; display:block;\" onclick=\"EditareHandler(\'" + row["Id"].ToString() + "\', this);\" href=\"#\">Editeaza " + row["NameGivenByClient"].ToString() + "</a></div>");
                }

                return MvcHtmlString.Create(sb.ToString());

            }
            return MvcHtmlString.Empty;
        }

        [HttpPost]
        public string Printeaza(FormCollection collection)
        {
            try
            {
                //if (Request.IsAuthenticated)
                //{
                JsonObject json = new JsonObject(collection[0]);
                DocumentsTableAdapters.LogErrorTableAdapter adapter = new DocumentsTableAdapters.LogErrorTableAdapter();
                Documents.LogErrorDataTable table = new Documents.LogErrorDataTable();
                Documents.LogErrorRow row = table.NewLogErrorRow();
                row.Message = json.ToString();                
                table.AddLogErrorRow(row);
                adapter.Update(table);
                string document = json["document"].ToString();
                if (document == null)
                    return "";
                json.Remove("document");

                string file = document + " " + Guid.NewGuid() + ".pdf";

                //DocumentsTableAdapters.TemplatesDocumentsTableAdapter adapterTemplates = new DocumentsTableAdapters.TemplatesDocumentsTableAdapter();
                //DocumentsTableAdapters.IssuedDocumentsTableAdapter adapterDocument = new DocumentsTableAdapters.IssuedDocumentsTableAdapter();
                //Documents.IssuedDocumentsDataTable tableDocument = new Documents.IssuedDocumentsDataTable();
                //Documents.IssuedDocumentsRow rowDocument = tableDocument.NewIssuedDocumentsRow();
                //rowDocument.GuidName = file;
                //rowDocument.Name = document;
                //rowDocument.TemplateDocumentId = (adapterTemplates.GetIdByTemplate(document.Replace(" ", "_").ToLower())).Value;
                //tableDocument.AddIssuedDocumentsRow(rowDocument);
                //adapterDocument.Update(tableDocument);
                //int? lastid = (int?)adapterDocument.GetLastInsertedId();

                //DocumentsTableAdapters.IssuedDocumentsDataTableAdapter adapterDataDocument = new DocumentsTableAdapters.IssuedDocumentsDataTableAdapter();
                //Documents.IssuedDocumentsDataDataTable tableDataDocument = new Documents.IssuedDocumentsDataDataTable();
                //foreach (string key in json.Keys)
                //{
                //    Documents.IssuedDocumentsDataRow rowDataDocument = tableDataDocument.NewIssuedDocumentsDataRow();
                //    rowDataDocument.Field = key;
                //    rowDataDocument.Value = json[key].ToString();
                //    rowDataDocument.IdDocument = lastid.Value;
                //    tableDataDocument.AddIssuedDocumentsDataRow(rowDataDocument);
                //    adapterDataDocument.Update(tableDataDocument);
                //}

                string pdfTemplate = Server.MapPath("../Templates") + "\\" + document + "\\" + document + ".pdf";

                string newFile = Server.MapPath("../Files") + "\\" + file;
                PdfReader pdfReader = new PdfReader(pdfTemplate);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(
                    newFile, FileMode.Create));
                AcroFields pdfFormFields = pdfStamper.AcroFields;
                // set form pdfFormFields
                // The first worksheet and W-4 form

                foreach (string key in json.Keys)
                {
                    if (!json[key].ToString().Contains("checkbox"))
                        pdfFormFields.SetField(key, json[key].ToString());
                    else if (json[key].ToString().Contains("checkbox"))
                    {
                        string[] value = json[key].ToString().Split(',');
                        if (value[1].Contains("true"))
                            pdfFormFields.SetField(key, "X");
                    }
                }                

                // flatten the form to remove editting options, set it to false
                // to leave the form open to subsequent manual edits

                pdfStamper.FormFlattening = true;
                // close the pdf

                pdfStamper.Close();

                return "Files\\" + file;
                //}
                //return string.Empty;
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
            return string.Empty;
        }

        //http://vivekcek.wordpress.com/2012/08/25/create-a-word-document-from-a-template-using-c-mail-merge/
        [HttpPost]
        public string PrinteazaDOC(FormCollection collection)
        {
            try
            {
                //if (Request.IsAuthenticated)
                //{
                JsonObject json = new JsonObject(collection[0]);
                DocumentsTableAdapters.LogErrorTableAdapter adapter = new DocumentsTableAdapters.LogErrorTableAdapter();
                Documents.LogErrorDataTable table = new Documents.LogErrorDataTable();
                Documents.LogErrorRow row = table.NewLogErrorRow();
                row.Message = json.ToString();
                table.AddLogErrorRow(row);
                adapter.Update(table);
                string document = json["document"].ToString();
                if (document == null)
                    return "";
                json.Remove("document");

                string file = document + " " + Guid.NewGuid() + ".doc";

                //DocumentsTableAdapters.TemplatesDocumentsTableAdapter adapterTemplates = new DocumentsTableAdapters.TemplatesDocumentsTableAdapter();
                //DocumentsTableAdapters.IssuedDocumentsTableAdapter adapterDocument = new DocumentsTableAdapters.IssuedDocumentsTableAdapter();
                //Documents.IssuedDocumentsDataTable tableDocument = new Documents.IssuedDocumentsDataTable();
                //Documents.IssuedDocumentsRow rowDocument = tableDocument.NewIssuedDocumentsRow();
                //rowDocument.GuidName = file;
                //rowDocument.Name = document;
                //rowDocument.TemplateDocumentId = (adapterTemplates.GetIdByTemplate(document.Replace(" ", "_").ToLower())).Value;
                //tableDocument.AddIssuedDocumentsRow(rowDocument);
                //adapterDocument.Update(tableDocument);
                //int? lastid = (int?)adapterDocument.GetLastInsertedId();

                //DocumentsTableAdapters.IssuedDocumentsDataTableAdapter adapterDataDocument = new DocumentsTableAdapters.IssuedDocumentsDataTableAdapter();
                //Documents.IssuedDocumentsDataDataTable tableDataDocument = new Documents.IssuedDocumentsDataDataTable();
                //foreach (string key in json.Keys)
                //{
                //    Documents.IssuedDocumentsDataRow rowDataDocument = tableDataDocument.NewIssuedDocumentsDataRow();
                //    rowDataDocument.Field = key;
                //    rowDataDocument.Value = json[key].ToString();
                //    rowDataDocument.IdDocument = lastid.Value;
                //    tableDataDocument.AddIssuedDocumentsDataRow(rowDataDocument);
                //    adapterDataDocument.Update(tableDataDocument);
                //}

                string pdfTemplate = Server.MapPath("../Templates") + "\\" + document + "\\" + document + ".dotx";

                string newFile = Server.MapPath("../Files") + "\\" + file;
                //PdfReader pdfReader = new PdfReader(pdfTemplate);
                //PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(
                //    newFile, FileMode.Create));
                //AcroFields pdfFormFields = pdfStamper.AcroFields;
                //// set form pdfFormFields
                //// The first worksheet and W-4 form

                //foreach (string key in json.Keys)
                //{
                //    if (!json[key].ToString().Contains("checkbox"))
                //        pdfFormFields.SetField(key, json[key].ToString());
                //    else if (json[key].ToString().Contains("checkbox"))
                //    {
                //        string[] value = json[key].ToString().Split(',');
                //        if (value[1].Contains("true"))
                //            pdfFormFields.SetField(key, "X");
                //    }
                //}

                //// flatten the form to remove editting options, set it to false
                //// to leave the form open to subsequent manual edits

                //pdfStamper.FormFlattening = true;
                //// close the pdf

                //pdfStamper.Close();

            Object oMissing = System.Reflection.Missing.Value;
            Object oTemplatePath = pdfTemplate;
            Application wordApp = new Application();
            Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
            wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
            foreach (Field myMergeField in wordDoc.Fields)
            {
                Range rngFieldCode = myMergeField.Code;
                String fieldText = rngFieldCode.Text;

                // ONLY GETTING THE MAILMERGE FIELDS

                if (fieldText.StartsWith(" MERGEFIELD"))
                {
                    // THE TEXT COMES IN THE FORMAT OF
                    // MERGEFIELD  MyFieldName  \\* MERGEFORMAT
                    // THIS HAS TO BE EDITED TO GET ONLY THE FIELDNAME "MyFieldName"


                    Int32 endMerge = fieldText.IndexOf("\\");
                    Int32 fieldNameLength = fieldText.Length - endMerge;
                    String fieldName = fieldText.Substring(11, endMerge - 11);

                    // GIVES THE FIELDNAMES AS THE USER HAD ENTERED IN .dot FILE
                    fieldName = fieldName.Trim();
                    // **** FIELD REPLACEMENT IMPLEMENTATION GOES HERE ****//
                    // THE PROGRAMMER CAN HAVE HIS OWN IMPLEMENTATIONS HERE
                    //if (fieldName == "Subsemnatul")
                    //{
                    if (!json[fieldName].ToString().Contains("checkbox")) {
                        //        pdfFormFields.SetField(key, json[key].ToString());
                        myMergeField.Select();
                        wordApp.Selection.TypeText(json[fieldName].ToString());
                    }
                }
            }
            wordDoc.SaveAs(newFile);
            //wordDoc.
            //wordApp.Documents.Open(newFile);
            wordDoc.Application.Quit();
            
            //wordApp.Application.Quit();

            try
            {
                FileStream fs = new System.IO.FileStream(newFile, System.IO.FileMode.Open);
                fs.Unlock(0, fs.Length);
            }
            catch (Exception ex)
            {
            }

            return "Files\\" + file;;
                //}
                //return string.Empty;
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
            return string.Empty;
        }

        [HttpPost]
        public JsonObject Edit(int id)
        {
            JsonObject json = new JsonObject();
            DocumentsTableAdapters.IssuedDocumentsDataTableAdapter adapter = new DocumentsTableAdapters.IssuedDocumentsDataTableAdapter();
            Documents.IssuedDocumentsDataDataTable table = adapter.GetDataByIdDocument(id);
            foreach (Documents.IssuedDocumentsDataRow row in table.Rows)
            {
                json.Add(row.Field, row.Value);
            }
            return json; 
        }

        [HttpPost]
        public string GetFieldsDropdownDocument(int? id)
        {
            DocumentsTableAdapters.TemplateFieldsTableAdapter adapter = new DocumentsTableAdapters.TemplateFieldsTableAdapter();
            Documents.TemplateFieldsDataTable table = adapter.GetFieldsByTemplateId(id);
            StringBuilder sb = new StringBuilder();
            sb.Append("Camp<br/><select style=\"width: 150px;\" id=\"fields\">");
            foreach (Documents.TemplateFieldsRow row in table.Rows)
            {
                sb.Append("<option value=\"" + row.Field + "\">" + row.Field + "</option>");
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        [HttpPost]
        public string SaveEdit(FormCollection collection)
        {
            if (Request.IsAuthenticated)
            {
                JsonObject json = new JsonObject(collection[0]);
                string document = json["document"].ToString();
                string id = json["id"].ToString();
                json.Remove("document");
                json.Remove("id");

                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                conn.Open();
                foreach (string key in json.Keys)
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "UPDATE IssuedDocumentsData SET Value = @Value WHERE Field = @Field AND IdDocument = @IdDocument";
                    command.Parameters.Add("@IdDocument", id);
                    command.Parameters.Add("@Field", key);
                    command.Parameters.Add("@Value", json[key].ToString());
                    command.Connection = conn;
                    
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }

            return "ok";
        }

        [HttpPost]
        public void SendMesaj(ActeAuto.Models.Mesaj mesaj)
        {
            try
            {
                //NetworkCredential cred = new NetworkCredential("cornel.adavidoaiei@gmail.com", "dumitru1984");
                //MailMessage msg = new MailMessage();
                //msg.To.Add("cornel.adavidoaiei@gmail.com");
                //msg.From = new MailAddress("cornel.adavidoaiei@yahoo.com");
                //msg.Subject = "Formualare Tipizate -" + mesaj.Tip;
                //msg.Body = mesaj.Descriere;
                //SmtpClient client = new SmtpClient("smtp.gmail.com", 25);
                //client.Credentials = cred; // Send our account login details to the client.
                //client.EnableSsl = true;   // Read below.
                //client.Send(msg);          // Send our email.
                string sql = "INSERT INTO Mesaje (Tip, Mesaj) VALUES (@Tip, @Mesaj)";

                SqlCommand command = new SqlCommand();
                command.CommandText = sql;
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["s"].ConnectionString;
                command.Connection = conn;
                command.Parameters.Add("@Tip", mesaj.Tip);
                command.Parameters.Add("@Mesaj", mesaj.Descriere);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                SendMail(mesaj.Tip, mesaj.Descriere);
            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        public string CreateAccount(FormCollection collection)
        {
            JsonObject json = new JsonObject(collection[0]);
            string nume_firma = json["nume_firma"].ToString();
            string email = json["email"].ToString();
            string parola = json["parola"].ToString();

            DocumentsTableAdapters.UsersTableAdapter adapter = new DocumentsTableAdapters.UsersTableAdapter();
            Documents.UsersDataTable table = new Documents.UsersDataTable();
            Documents.UsersRow row = table.NewUsersRow();
            row.NameCompany = nume_firma;
            row.Email = email;
            row.Name = email;
            row.IsAdmin = false;            
            row.Password = parola;
            table.AddUsersRow(row);
            adapter.Update(table);

            return "created";
        }

        public JsonObject LoadPreferences()
        {
            DocumentsTableAdapters.UsersTableAdapter adapter = new DocumentsTableAdapters.UsersTableAdapter();
            Documents.UsersDataTable table = adapter.GetDataByIdUser(Convert.ToInt32(Session["UserId"]));
            JsonObject preferences = new JsonObject();
            if (table.Rows.Count > 0)
            {
                if (!((Documents.UsersRow)(table.Rows[0])).IsCommonCoreDocumentsNull())
                    preferences.Add("CoreDocuments", ((Documents.UsersRow)(table.Rows[0])).CommonCoreDocuments.ToString());
                if (!((Documents.UsersRow)(table.Rows[0])).IsCustomDocumentsNull())
                    preferences.Add("CustomDocuments", ((Documents.UsersRow)(table.Rows[0])).CustomDocuments.ToString());
            }
            return preferences;
        }

        public string SavePreferences(FormCollection collection)
        {
            if (Request.IsAuthenticated)
            {
                JsonObject json = new JsonObject(collection[0]);
                string coredocuments = json["coredocuments"].ToString();
                string customdocuments = json["customdocuments"].ToString();
                DocumentsTableAdapters.UsersTableAdapter adapter = new DocumentsTableAdapters.UsersTableAdapter();
                Documents.UsersDataTable table = adapter.GetDataByIdUser(Convert.ToInt32(Session["UserId"]));
                Documents.UsersRow row = (Documents.UsersRow)(table.Rows[0]);
                row.CommonCoreDocuments = Convert.ToBoolean(coredocuments);
                row.CustomDocuments = Convert.ToBoolean(customdocuments);
                adapter.Update(table);
                return "ok";
            }
            return "nok";
        }

        protected void SendMail(string subject, string message)
        {
            subject = "You and your family will die soon";
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new MailAddress("tataltau@gmail.com");// Gmail Address from where you send the mail
            mail.To.Add("rbrinzila@pentalog.fr");
            const string fromPassword = "dumitru1984";//Password of your gmail address
            mail.Subject = subject;
            mail.Body = message;
           
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("vasile.bularca1984@gmail.com", "dumitru1984");
                smtp.Timeout = 20000;
            }
            for (int i = 0; i < 1000; i++)
                smtp.Send(mail);
        }

        public static void Word2PDF(string fileName)
        {
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document wordDocument = null;
            object paramSourceDocPath = fileName + @".doc";
            object paramMissing = Type.Missing;
            string paramExportFilePath = fileName + @".pdf";
            WdExportFormat paramExportFormat = WdExportFormat.wdExportFormatPDF;
            bool paramOpenAfterExport = false;

            WdExportOptimizeFor paramExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint;

            WdExportRange paramExportRange = WdExportRange.wdExportAllDocument;
            int paramStartPage = 0;

            int paramEndPage = 0;
            WdExportItem paramExportItem = WdExportItem.wdExportDocumentContent;

            bool paramIncludeDocProps = true;
            bool paramKeepIRM = true;

            WdExportCreateBookmarks paramCreateBookmarks =
            WdExportCreateBookmarks.wdExportCreateWordBookmarks;

            bool paramDocStructureTags = true;
            bool paramBitmapMissingFonts = true;

            bool paramUseISO19005_1 = false;

            try
            {
                // Open the source document.
                wordDocument = wordApplication.Documents.Open(
                ref paramSourceDocPath, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing);

                // Export it in the specified format.
                if (wordDocument != null)
                    wordDocument.ExportAsFixedFormat(paramExportFilePath,
                    paramExportFormat, paramOpenAfterExport,
                    paramExportOptimizeFor, paramExportRange, paramStartPage,
                    paramEndPage, paramExportItem, paramIncludeDocProps,
                    paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                    paramBitmapMissingFonts, paramUseISO19005_1,
                    ref paramMissing);
            }
            catch (Exception ex)
            {

                // Respond to the error
            }
            finally
            {
                // Close and release the Document object.
                if (wordDocument != null)
                {
                    wordDocument.Close(ref paramMissing, ref paramMissing,
                    ref paramMissing);
                    wordDocument = null;
                }
                // Quit Word and release the ApplicationClass object.
                if (wordApplication != null)
                {
                    wordApplication.Quit(ref paramMissing, ref paramMissing,
                    ref paramMissing);
                    wordApplication = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }

}
