<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="Administrare.CreateUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>Nume</td>
            <td>
                <asp:TextBox ID="tbNume" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Parola</td>
            <td>
                <asp:TextBox ID="tbParola" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>IsAdmin</td>
            <td>
                <asp:CheckBox ID="cbIsAdmin" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Email</td>
            <td>
                <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Nume Companie</td>
            <td>
                <asp:TextBox ID="tbNumeCompanie" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnCreateUser" runat="server" Text="Adauga" OnClick="btnCreateUser_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
