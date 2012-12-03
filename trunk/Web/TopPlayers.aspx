<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TopPlayers.aspx.cs" Inherits="Web.TopPlayers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridViewTopPlayers" runat="server" AutoGenerateColumns="False"
        DataSourceID="SqlDataSourceTopTenPlayers" CellPadding="4" 
        ForeColor="#333333" GridLines="None" 
        onrowcreated="GridViewTopPlayers_RowCreated">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="RowNumberLabel" runat="server">
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="Player Name" SortExpression="Name" />
            <asp:BoundField DataField="LEVEL" HeaderText="Level" SortExpression="LEVEL" />
            <asp:BoundField DataField="Class" HeaderText="Class" ReadOnly="True" SortExpression="Class" />
            <asp:BoundField DataField="Connected" HeaderText="Status" ReadOnly="True" SortExpression="Connected" />
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSourceTopTenPlayers" runat="server" ConnectionString="<%$ ConnectionStrings:XiahDb %>"
        ProviderName="<%$ ConnectionStrings:XiahDb.ProviderName %>" 
        SelectCommand="SELECT TopTenPlayers.* FROM TopTenPlayers">
    </asp:SqlDataSource>
</asp:Content>
