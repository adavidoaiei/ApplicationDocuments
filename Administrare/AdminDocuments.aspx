<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminDocuments.aspx.cs" Inherits="Administrare.AdminDocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAdaugaDocument" />
        </Triggers>
        <ContentTemplate>
            <div style="margin-top: 20px;">
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Exception" />
                <div style="float: left; margin-right: 20px;">
                    <asp:LinkButton ID="lbAdaugaDocument" runat="server" OnClick="lbAdaugaDocument_Click">Adauga Document</asp:LinkButton><br />
                    <br />
                    Categorie Document
        <asp:DropDownList ID="ddlFiltruCategorie" runat="server" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="Category" DataValueField="Id" OnSelectedIndexChanged="ddlCategorie_SelectedIndexChanged">
            <asp:ListItem>Toate</asp:ListItem>
        </asp:DropDownList>
                    <asp:GridView ID="grdDocuments" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlDataSource1" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None">
                        <AlternatingRowStyle BackColor="PaleGoldenrod" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEdit" runat="server" CommandArgument='<%# Eval("Id") %>' OnCommand="lbEdit_Command">Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                            <asp:BoundField DataField="Name" HeaderText="Nume Document" SortExpression="Name" />
                            <asp:BoundField DataField="Category" HeaderText="Categorie" SortExpression="Name" />
                            <asp:BoundField DataField="InProd" HeaderText="InProd" SortExpression="InProd" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbGoProd" runat="server" CommandArgument='<%# Eval("Id") %>' OnCommand="lbGoProd_Command" OnClientClick="return confirm('Sunteti sigur ca doriti sa puneti sablonul in productie ?');">Go Prod</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbSterge" runat="server" CommandArgument='<%# Eval("Id") %>' OnCommand="lbSterge_Command" OnClientClick="return confirm('Sunteti sigur ca doriti sa stergeti sablonul ?');">Sterge</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="Tan" />
                        <HeaderStyle BackColor="Tan" Font-Bold="True" />
                        <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                        <SortedAscendingCellStyle BackColor="#FAFAE7" />
                        <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                        <SortedDescendingCellStyle BackColor="#E1DB9C" />
                        <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                    </asp:GridView>
                    <br />
                </div>
                <div style="float: left;">
                    <table>
                        <tr>
                            <td colspan="2"><asp:Label ID="lblError" runat="server" ForeColor="Red" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Nume Firma</td>
                            <td>
                                <asp:DropDownList ID="ddlNumeFirme" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSource3" DataTextField="NameCompany" DataValueField="Id">
                                    <asp:ListItem Value="-1">Trunchi Comun</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Categorie
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCategorii" runat="server" DataSourceID="SqlDataSource2" DataTextField="Category" DataValueField="Id"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Nume Document
                            </td>
                            <td>
                                <asp:TextBox ID="tbNumeDocument" runat="server"></asp:TextBox><br />                                
                            </td>
                        </tr>
                        <tr>
                            <td>DOC Document</td>
                            <td>
                                <asp:FileUpload ID="fuDocDocument" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>HTML Document</td>
                            <td>
                                <asp:FileUpload ID="fuHtmlDocument" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>PDF Document</td>
                            <td>
                                <asp:FileUpload ID="fuPdfDocument" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Imagine Document</td>
                            <td>
                                <asp:FileUpload ID="fuImagineDocument" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnAdaugaDocument" runat="server" Text="Adauga Document" OnClick="btnAdaugaDocument_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" >
        <ProgressTemplate>
            In Progress
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT * FROM [Categories] ORDER BY Category ASC"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT Id, NameCompany FROM Users WHERE (IsAdmin = 'false') ORDER BY NameCompany"></asp:SqlDataSource>
</asp:Content>
