<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DreamyTasty.User.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblMsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </div>
                <h2>Login</h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form_container">
                        <img id="userLogin" src="../Images/welcome.jpg" alt="" class="img-thumbnail" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form_container">
                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ErrorMessage="Username is required" ControlToValidate="txtUsername"
                            ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter Username"></asp:TextBox>
                    </div>
                    <div class="form_container">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Password is required" ControlToValidate="txtPassword"
                            ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
                        <div class="show-password">
                            <input type="checkbox" id="chkShowPassword" onclick="togglePasswordVisibility(this)">
                            <label for="chkShowPassword" class="show-password-label" font-size="Small">Show Password</label>
                        </div>
                    </div>

                    <style>
                        .show-password {
                            margin-top: 5px;
                        }

                            .show-password input[type="checkbox"] {
                                margin-right: 5px; 
                            }
                    </style>

                    <script>
                        function togglePasswordVisibility(checkbox) {
                            var passwordInput = document.getElementById('<%= txtPassword.ClientID %>');
                            if (checkbox.checked) {
                                passwordInput.type = 'text';
                                document.querySelector('.show-password-label').textContent = 'Hide Password';
                            } else {
                                passwordInput.type = 'password';
                                document.querySelector('.show-password-label').textContent = 'Show Password';
                            }
                        }
                    </script>

                    <div class="btn-box">
                        <asp:Button ID="btnLogin" runat="server" Text="Login   " CssClass="btn btn-success rounded-pill p-l-4 pl-4 text-white" OnClick="btnLogin_Click" />
                        <span class="pl-3 text-info">Don't have an account? <a href="Registration.aspx" class="badge badge-info">Register</a></span>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
