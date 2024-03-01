using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DreamyTasty.User
{
    public partial class Contact : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("ContactSp", con);
                cmd.Parameters.AddWithValue("@Action", "INSERT");
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Subject", txtSubject.Text.Trim());
                cmd.Parameters.AddWithValue("@Message", txtMessage.Text.Trim());
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                lblMsg.Visible = true;
                lblMsg.Text = "Thanks for your contact! We will enter in contact as soon as possible!";
                lblMsg.CssClass = "alert alert-success";
                clear();
            }
            catch (Exception ex) 
            {
                Response.Write("<script>alert(' " + ex.Message + " '); </script>");
            }
            finally { con.Close(); }
        }

        private void clear()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtMessage.Text = string.Empty;
        }
    }
}