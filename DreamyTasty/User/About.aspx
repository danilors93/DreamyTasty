<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="DreamyTasty.User.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- about section -->

  <section class="about_section layout_padding">
    <div class="container  ">

      <div class="row">
        <div class="col-md-6 ">
          <div class="img-box">
            <img src="../TemplateFiles/images/about-img.png" alt="">
          </div>
        </div>
        <div class="col-md-6">
          <div class="detail-box">
            <div class="heading_container">
              <h2>
                We are Dremy Tasty
              </h2>
            </div>
            <p>
              Welcome to Dreamy Tasty, where culinary dreams come to life! Nestled in the heart of our bustling city, Dreamy Tasty is more than just a restaurant; it's a gastronomic journey that tantalizes your taste buds and ignites your imagination.

At Dreamy Tasty, we believe in crafting unforgettable dining experiences that leave a lasting impression. Our talented chefs, inspired by flavors from around the globe, meticulously curate each dish to perfection. From mouthwatering appetizers to indulgent main courses and delectable desserts, every bite tells a story of passion and creativity.

But Dreamy Tasty is more than just a feast for the senses; it's a place where friends and family come together to create cherished memories. Our warm and inviting ambiance sets the stage for laughter, conversation, and moments of pure joy.

Whether you're celebrating a special occasion or simply craving a culinary adventure, Dreamy Tasty welcomes you with open arms. Join us and embark on a gastronomic odyssey that will leave you craving more. Come, experience the magic of Dreamy Tasty today!
            </p>
            <%--<a href="">
              Read More
            </a>--%>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- end about section -->
</asp:Content>
