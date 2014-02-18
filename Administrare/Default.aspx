<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Administrare._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">

</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div style="margin-left: auto; margin-right: auto; width: 310px; margin-top: 50px;">
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text=""></asp:Label><br />
        Nume<br />
        <asp:TextBox ID="tbUser" runat="server"></asp:TextBox><br />
        Parola<br />
        <asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox><br />
        <div style="float: right;">
            <asp:Button ID="btnLogin" runat="server" Text="Autentifica" OnClick="btnLogin_Click" />
        </div>
        <br style="clear: both;" />
    </div>
</asp:Content>
