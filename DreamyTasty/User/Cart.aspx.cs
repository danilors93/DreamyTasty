using DreamyTasty.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DreamyTasty.User
{
    public partial class Cart : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        decimal grandTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    getCartItems();
                }
            }
        }
        void getCartItems()
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rCartItem.DataSource = dt;
            if (dt.Rows.Count == 0)
            {
                rCartItem.FooterTemplate = null;
                rCartItem.FooterTemplate = new CustomTemplate(ListItemType.Footer);
            }
            rCartItem.DataBind();
        }
        protected void rCartItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Utils utils = new Utils();
            if (e.CommandName == "remove")
            {
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Cart_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    getCartItems();
                    Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["userId"]));
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error - " + ex.Message + " ');<script>");
                }
                finally
                {
                    con.Close();
                }
            }
            if (e.CommandName == "updateCart")
            {
                bool isCartUpdated = false;
                for (int item = 0; item < rCartItem.Items.Count; item ++)
                {
                    if (rCartItem.Items[item].ItemType == ListItemType.Item || rCartItem.Items[item].ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox quantity = rCartItem.Items[item].FindControl("txtQuantity") as TextBox;
                        HiddenField _productId = rCartItem.Items[item].FindControl("hdnProductId") as HiddenField;
                        HiddenField _quantity = rCartItem.Items[item].FindControl("hdnQuantity") as HiddenField;
                        int quantityFromCart = Convert.ToInt32(quantity.Text);
                        int ProductId = Convert.ToInt32(_productId.Value);
                        int quantityFromDB = Convert.ToInt32(_quantity.Value);
                        bool isTrue = false;
                        int updateQuantity = 1;
                        if (quantityFromCart > quantityFromDB)
                        {
                            updateQuantity = quantityFromCart;
                            isTrue = true;
                        }
                        else if (quantityFromCart < quantityFromDB)
                        {
                            updateQuantity = quantityFromCart;
                            isTrue = true;
                        }
                        if (isTrue)
                        {
                            isCartUpdated = utils.updateCartQuantity(updateQuantity, ProductId, Convert.ToInt32(Session["userId"]));
                        }
                    }
                }
                getCartItems();
            }
            if (e.CommandName == "checkout")
            {
                bool isTrue = false;
                string pName = string.Empty;
                for (int item = 0; item < rCartItem.Items.Count; item ++)
                {
                    if (rCartItem.Items[item].ItemType == ListItemType.Item || rCartItem.Items[item].ItemType == ListItemType.AlternatingItem)
                    {
                        HiddenField _productId = rCartItem.Items[item].FindControl("hdnProductId") as HiddenField;
                        HiddenField _cartQuantity = rCartItem.Items[item].FindControl("hdnQuantity") as HiddenField;
                        HiddenField _productQuantity = rCartItem.Items[item].FindControl("hdnPrdQuantity") as HiddenField;
                        Label productName = rCartItem.Items[item].FindControl("lblName") as Label;
                        int productId = Convert.ToInt32(_productId.Value);
                        int cartQuantity = Convert.ToInt32(_cartQuantity.Value);
                        int productQuantity = Convert.ToInt32(_productQuantity.Value);
                        if (productQuantity > cartQuantity && productQuantity > 2)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            pName = productName.Text.ToString();
                            break;
                        }
                    }
                }

                if (isTrue)
                {
                    Response.Redirect("Payment.aspx");
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "We don't have that ammount of <b>'" + pName + "'</b> left in stock! :(";
                    lblMsg.CssClass = "alet alert-warning";
                }
            }
        }
        private int GetProductQuantity(int productId)
        {
            int quantity = 0;

            using (SqlConnection con = new SqlConnection(Connection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Quantity FROM Products WHERE ProductId = @ProductId", con))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    con.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        quantity = Convert.ToInt32(result);
                    }
                }
            }

            return quantity;
        }

        protected void rCartItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label totalPrice = e.Item.FindControl("lblTotalPrice") as Label;
                Label productPrice = e.Item.FindControl("lblPrice") as Label;
                TextBox quantity = e.Item.FindControl("txtQuantity") as TextBox;
                decimal calTotalPrice = Convert.ToDecimal(productPrice.Text) * Convert.ToDecimal(quantity.Text);
                totalPrice.Text = calTotalPrice.ToString();
                grandTotal += calTotalPrice;
            }
            Session["grandTotalPrice"] = grandTotal;
        }

        private sealed class CustomTemplate : ITemplate
        {
            private ListItemType ListItemType { get; set; }
            public CustomTemplate(ListItemType type)
            {
                ListItemType = type;
            }
            public void InstantiateIn(Control container)
            {
                if (ListItemType == ListItemType.Footer)
                {
                    var footer = new LiteralControl("<tr><td colspan='5'><b>Your Cart is empity.</b><a href='Menu.aspx' class='badge badge-info ml-2'>Continue Shopping</a></td></tr></tbody></table>");
                    container.Controls.Add(footer);
                }
            }
        }
    }
}