using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Administrare
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT Id FROM Users WHERE Name = @Name AND Password = @Password AND IsAdmin = 'true'";
            command.Parameters.Add("@Name", tbUser.Text);
            command.Parameters.Add("@Password", tbPassword.Text);
            command.Connection = new SqlConnection();
            command.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            command.Connection.Open();
            int? id = (int?)command.ExecuteScalar();
            command.Connection.Close();
            if (id > 0)
            {
                Session["IsLogged"] = true;
                Session["UserName"] = tbUser.Text;
                Session["UserId"] = id;
                Response.Redirect("~/AdminDocuments.aspx");
            }
            else
            {
                lblError.Text = "User sau parola incorecta";
            }
        }
    }
}