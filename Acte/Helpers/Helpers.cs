using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.Web.Hosting;
using System.Data;
using System.Data.SqlClient;

namespace ActeAuto.Helpers
{
    public static class Helpers
    {
        public static MvcHtmlString DropDownDocumente(this HtmlHelper helper, int? idUser, string url)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<select onchange=\"changeDocument()\" id=\"select\">" + System.Environment.NewLine);

                string sql = "SELECT Id, Category FROM Categories";
                DataTable categories = GetDataTable(sql);
                foreach (DataRow rowCategorie in categories.Rows)
                {
                    sb.Append("<optgroup label=\"" + rowCategorie["Category"].ToString() + "\">" + System.Environment.NewLine);
                    string sqlDoc = "SELECT Id, Name FROM TemplatesDocuments WHERE IdCategorie = '" + rowCategorie["Id"].ToString() + "'";                    
                    
                    if (idUser != null && idUser != 0)
                    {
                        DocumentsTableAdapters.UsersTableAdapter adapterPreferences = new DocumentsTableAdapters.UsersTableAdapter();
                        Documents.UsersDataTable preferenceTable = adapterPreferences.GetDataByIdUser(idUser.Value);
                        if (preferenceTable.Count > 0)
                        {
                            Documents.UsersRow preference = (Documents.UsersRow)preferenceTable.Rows[0];
                            if (!preference.IsCommonCoreDocumentsNull() && !preference.IsCustomDocumentsNull())
                            {
                                if (preference.CommonCoreDocuments == true && preference.CustomDocuments == true)
                                {
                                    sqlDoc += " AND ((UserId IS NULL OR UserId = -1) OR (UserId = " + idUser + "))";
                                }
                                if (preference.CommonCoreDocuments == true && preference.CustomDocuments == false)
                                {
                                    sqlDoc += " AND ((UserId IS NULL OR UserId = -1))";
                                }
                                if (preference.CommonCoreDocuments == false && preference.CustomDocuments == true)
                                {
                                    sqlDoc += " AND ((UserId = " + idUser + "))";
                                }
                                if (url.Contains("staging") == false && url.Contains("localhost") == false)
                                {
                                    sqlDoc += " AND InProd = 'true'";
                                }
                            }
                        }
                    }
                    DataTable documents = GetDataTable(sqlDoc);
                    foreach (DataRow drDocument in documents.Rows)
                    {
                        sb.Append("<option value=\"" + drDocument["Id"].ToString() + "\">" + drDocument["Name"].ToString() + "</option>");
                    }
                    sb.Append("</optgroup>" + System.Environment.NewLine);
                }
                sb.Append("</select>");

                return MvcHtmlString.Create(sb.ToString());
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

                return MvcHtmlString.Create("");
            }       
        }

        public static MvcHtmlString DropDownDocumenteSearch(this HtmlHelper helper, int? idUser, string url)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<select onchange=\"changeSearch()\" id=\"selectSearch\" style=\"width: 150px;\">" + System.Environment.NewLine);

                string sql = "SELECT Id, Category FROM Categories";
                DataTable categories = GetDataTable(sql);
                foreach (DataRow rowCategorie in categories.Rows)
                {
                    sb.Append("<optgroup label=\"" + rowCategorie["Category"].ToString() + "\">" + System.Environment.NewLine);
                    string sqlDoc = "SELECT Id, Name FROM TemplatesDocuments WHERE IdCategorie = '" + rowCategorie["Id"].ToString() + "' ";
                   
                    if (idUser != null && idUser != 0)
                    {
                        DocumentsTableAdapters.UsersTableAdapter adapterPreferences = new DocumentsTableAdapters.UsersTableAdapter();
                        Documents.UsersDataTable preferenceTable = adapterPreferences.GetDataByIdUser(idUser.Value);
                        if (preferenceTable.Count > 0)
                        {
                            Documents.UsersRow preference = (Documents.UsersRow)preferenceTable.Rows[0];
                            if (!preference.IsCommonCoreDocumentsNull() && !preference.IsCustomDocumentsNull())
                            {
                                if (preference.CommonCoreDocuments == true && preference.CustomDocuments == true)
                                {
                                    sqlDoc += " AND ((UserId IS NULL OR UserId = -1) OR (UserId = " + idUser + "))";
                                }
                                if (preference.CommonCoreDocuments == true && preference.CustomDocuments == false)
                                {
                                    sqlDoc += " AND ((UserId IS NULL OR UserId = -1))";
                                }
                                if (preference.CommonCoreDocuments == false && preference.CustomDocuments == true)
                                {
                                    sqlDoc += " AND ((UserId = " + idUser + "))";
                                }
                                if (url.Contains("staging") == false && url.Contains("localhost") == false)
                                {
                                    sqlDoc += " AND InProd = 'true'";
                                }
                            }
                        }
                    }
                    DataTable documents = GetDataTable(sqlDoc);
                    foreach (DataRow drDocument in documents.Rows)
                    {
                        sb.Append("<option value=\"" + drDocument["Id"].ToString() + "\">" + drDocument["Name"].ToString() + "</option>");
                    }
                    sb.Append("</optgroup>" + System.Environment.NewLine);
                }

                sb.Append("</select>");

                return MvcHtmlString.Create(sb.ToString());
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

                return MvcHtmlString.Create("");
            }     
        }

        public static DataTable GetDataTable(string sql)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand();
            adapter.SelectCommand.CommandText = sql;
            adapter.SelectCommand.Connection = new SqlConnection();
            adapter.SelectCommand.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds.Tables[0];
        }
    }
}