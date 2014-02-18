using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ActeAuto.Models
{
    public class User
    {
        [Required(ErrorMessage = "Campul Utilizator Obligatoriu")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Campul Parola Obligatoriu")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// Checks if user with given password exists in the database
        /// </summary>
        /// <param name="_username">User name</param>
        /// <param name="_password">User password</param>
        /// <returns>True if user exist and password is correct</returns>
        public bool IsValid(string _username, string _password)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT Id FROM Users WHERE Name = @Name AND Password = @Password";
            command.Parameters.Add("@Name", _username);
            command.Parameters.Add("@Password", _password);
            command.Connection = new SqlConnection();
            command.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            command.Connection.Open();
            int? id = (int?)command.ExecuteScalar();
            command.Connection.Close();
            System.Web.HttpContext.Current.Session["UserId"] = id;
            if (id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}