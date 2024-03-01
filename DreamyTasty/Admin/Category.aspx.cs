using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DreamyTasty.Admin
{
    public partial class Category : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Category";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");
                }
                else
                {
                getCategories();
                }
            }
            lblMsg.Visible = false;
        }
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidtoExecute = true;
            int categoryId = Convert.ToInt32(hdnId.Value);
          
            using (SqlConnection con = new SqlConnection(Connection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Category_Crud", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);

                    if (fuCategoryImage.HasFile)
                    {
                        if (Utils.isValidExtension(fuCategoryImage.FileName))
                        {
                            Guid obj = Guid.NewGuid();
                            fileExtension = Path.GetExtension(fuCategoryImage.FileName);
                            imagePath = "Images/Category/" + obj.ToString() + fileExtension;
                            fuCategoryImage.PostedFile.SaveAs(Server.MapPath("~/Images/Category/") + obj.ToString() + fileExtension);
                            cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
                        }
                        else
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Please select a valid format";
                            lblMsg.CssClass = "alert alert-danger";
                            isValidtoExecute = false;
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                    }

                    if (isValidtoExecute)
                    {
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            actionName = categoryId == 0 ? "inserted" : "updated";
                            lblMsg.Visible = true;
                            lblMsg.Text = "<div class='card bg-success text-white'><div class='card-body'>Category " + actionName + " successfully!</div></div>";
                            lblMsg.CssClass = "alert alert-success";
                            getCategories();
                            clear();
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Error - " + ex.Message;
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                }
            }
        }
        private void getCategories()
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rCategory.DataSource = dt;
            rCategory.DataBind();
        }

        private void clear()
        {
            txtName.Text = string.Empty;
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOurUpdate.Text = "Add";
            imgCategory.ImageUrl = string.Empty;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void rCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            con = new SqlConnection(Connection.GetConnectionString());
            if (e.CommandName == "edit")
            {

                cmd = new SqlCommand("Category_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@CategoryId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                txtName.Text = dt.Rows[0]["Name"].ToString();
                cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                imgCategory.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageUrl"].ToString()) ?
                    "../Images/No_Image.png" : "../" + dt.Rows[0]["ImageUrl"].ToString();
                imgCategory.Height = 200;
                imgCategory.Width = 200;
                hdnId.Value = dt.Rows[0]["CategoryId"].ToString();
                btnAddOurUpdate.Text = "Update";
                LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                btn.CssClass = "badge - badge-warning";
            }
            else if (e.CommandName == "delete")
            {
                cmd = new SqlCommand("Category_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@CategoryId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Category deleted successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error - " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close();
                }
            }
        }


        protected void rCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbl = e.Item.FindControl("lblIsActive") as Label;
                if (lbl.Text == "True")
                {
                    lbl.Text = "Active";
                    lbl.CssClass = "badge badge-success";
                }
                else
                {
                    lbl.Text = "Inactive";
                    lbl.CssClass = "badge badge-danger";
                }
            }
        }
    }
}