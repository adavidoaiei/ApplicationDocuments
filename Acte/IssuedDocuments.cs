using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ActeAuto
{
    public class IssuedDocuments
    {
        public string GuidName { get; set; }
        public string Name { get; set;  }
        public int TemplateDocumentId { get; set; }
        public string NameGivenByClient { get; set; }
        public int UserId { get; set; }

        public int Save()
        {
            string sql = @"INSERT INTO [dbo].[IssuedDocuments]
                                   ([Name]
                                   ,[GuidName]
                                   ,[TemplateDocumentId]
                                   ,[NameGivenByClient]
                                   ,[UserId])
                             OUTPUT INSERTED.Id
                             VALUES
                                   (@Name
                                   ,@GuidName
                                   ,@TemplateDocumentId
                                   ,@NameGivenByClient
                                   ,@UserId)";

            SqlCommand command = new SqlCommand();
            command.Connection = new SqlConnection();
            command.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            command.CommandText = sql;
            command.Parameters.Add("@Name", Name);
            command.Parameters.Add("@GuidName", GuidName);
            command.Parameters.Add("@TemplateDocumentId", TemplateDocumentId);
            command.Parameters.Add("@NameGivenByClient", NameGivenByClient);
            command.Parameters.Add("@UserId", UserId);          
            command.Connection.Open();
            int id = (int)command.ExecuteScalar();
            command.Connection.Close();
            return id;
        }
    }
}