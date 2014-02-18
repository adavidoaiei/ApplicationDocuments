using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Administrare
{
    public class TemplatesDocuments
    {
        public string Name { get; set; }
        public int AddedBy { get; set; }
        public int IdCategorie { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public string PathTemplate { get; set; }
        public int UserId { get; set; }

        public int Save()
        {
            string sql = @"INSERT INTO [dbo].[TemplatesDocuments]
                           ([Name]                                                     
                           ,[AddedBy]
                           ,[IdCategorie]
                           ,[IsActive]
                           ,[CreationDate]
                           ,[CreatedBy]
                           ,[PathTemplate]
                           ,[UserId])
                     OUTPUT INSERTED.Id
                     VALUES
                           (@Name                                                      
                           ,@AddedBy
                           ,@IdCategorie
                           ,@IsActive
                           ,@CreationDate
                           ,@CreatedBy
                           ,@PathTemplate
                           ,@UserId);";

            SqlCommand command = new SqlCommand();
            command.Connection = new SqlConnection();
            command.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            command.CommandText = sql;
            command.Parameters.Add("@Name", Name);
            command.Parameters.Add("@AddedBy", AddedBy);
            command.Parameters.Add("@IdCategorie", IdCategorie);
            command.Parameters.Add("@IsActive", IsActive);
            command.Parameters.Add("@CreationDate", CreationDate);
            command.Parameters.Add("@CreatedBy", CreatedBy);
            command.Parameters.Add("@PathTemplate", PathTemplate);
            command.Parameters.Add("@UserId", UserId);
            command.Connection.Open();
            int id = (int)command.ExecuteScalar();
            command.Connection.Close();
            return id;
        }
    }
}