using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Administrare
{
    public partial class CreateUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            DocumentsTableAdapters.UsersTableAdapter adapter = new DocumentsTableAdapters.UsersTableAdapter();
            Documents.UsersDataTable table = new Documents.UsersDataTable();
            Documents.UsersRow row = table.NewUsersRow();
            row.Name = tbNume.Text;
            row.Email = tbEmail.Text;
            row.IsAdmin = cbIsAdmin.Checked;           
            row.Password = Cryptography.EncryptStringAES(tbParola.Text, "Dumitru_1984");
            row.NameCompany = tbNumeCompanie.Text;
            table.Rows.Add(row);
            adapter.Update(table);
        }
    }
}