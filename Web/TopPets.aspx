<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TopPets.aspx.cs" Inherits="Web.TopPets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridViewTopPets" runat="server" AutoGenerateColumns="False"
        DataSourceID="SqlDataSourceTopTenPets" CellPadding="4" 
        ForeColor="#333333" GridLines="None" 
        onrowcreated="GridViewTopPets_RowCreated">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="RowNumberLabel" runat="server">
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Owner Name" HeaderText="Owner" SortExpression="Owner" />
            <asp:BoundField DataField="Pet Name" HeaderText="Pet Name" SortExpression="Name" />
            <asp:BoundField DataField="Pet Level" HeaderText="Level" SortExpression="Level" />
            <asp:BoundField DataField="Pet Health" HeaderText="Health" SortExpression="Health" />
            <asp:BoundField DataField="Pet Damage" HeaderText="Damage" SortExpression="Damage" />
            <asp:BoundField DataField="Pet Defense" HeaderText="Defense" SortExpression="Defense" />
            <asp:BoundField DataField="Pet Attack Rating" HeaderText="Attack Rating" SortExpression="Attack Rating" />
            <asp:BoundField DataField="Pet Evolution" HeaderText="Evolution" SortExpression="Evolution" />
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
    <asp:SqlDataSource ID="SqlDataSourceTopTenPets" runat="server" ConnectionString="<%$ ConnectionStrings:XiahDb %>"
        ProviderName="<%$ ConnectionStrings:XiahDb.ProviderName %>" 
        SelectCommand="SELECT TopTenPets.* FROM TopTenPets">
    </asp:SqlDataSource>
</asp:Content>
