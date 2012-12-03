<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Web._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to XIAH WEB ETC
    </h2>
    <asp:LoginView ID="LoginView" runat="server">
        <AnonymousTemplate>
            <span style="font-size: large">Register account:</span>
            <br />
            Username:
            <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsername" runat="server" ErrorMessage="RequiredFieldValidator"
                Text="*" ControlToValidate="UsernameTextBox" ValidationGroup="InsertUserGroup"></asp:RequiredFieldValidator>
            <asp:Label ID="UsernameExistsLabel" runat="server" ForeColor="Red"></asp:Label>
            <br />
            Password:
            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" runat="server" ErrorMessage="RequiredFieldValidator"
                Text="*" ControlToValidate="PasswordTextBox" ValidationGroup="InsertUserGroup"></asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="ButtonOk" runat="server" Text="OK" OnClick="ButtonOk_Click" ValidationGroup="InsertUserGroup" />
        </AnonymousTemplate>
        <LoggedInTemplate>
            Change character nickname:
            <br />
            <asp:DropDownList ID="DropDownListCharactersChangeNickname" runat="server" 
                DataSourceID="ObjectDataSourceCharacters" DataTextField="Name" 
                DataValueField="CharacterId">
            </asp:DropDownList>
            <asp:ObjectDataSource ID="ObjectDataSourceCharacters" runat="server" 
                SelectMethod="GetAllCharactersByAccountId" TypeName="XiahBLL.CharacterManager">
                <SelectParameters>
                    <asp:SessionParameter Name="accountId" SessionField="UserId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br />
            New Nickname:
            <asp:TextBox ID="NewNicknameTextbox" runat="server" MaxLength="15"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredNewNickname" runat="server" ForeColor="Red"
                Text="*" ControlToValidate="NewNicknameTextBox" ValidationGroup="ChangeNicknameGroup"></asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="ChangeNicknameButton" runat="server" 
                ValidationGroup="ChangeNicknameGroup" onclick="ChangeNicknameButton_Click" 
                Text="OK" />
            <asp:Label ID="ChangeNicknameMessageLabel" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <hr />
            <br />
            Reset Skills:
            <br />
            <asp:DropDownList ID="DropDownListCharactersResetSkills" runat="server" 
                DataSourceID="ObjectDataSourceCharacters" DataTextField="Name" 
                DataValueField="CharacterId">
            </asp:DropDownList>
            <asp:Button ID="ResetSkillsButton" runat="server" Text="Reset" 
                onclick="ResetSkillsButton_Click" />
            <asp:Label ID="ResetSkillsMessageLabel" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <hr />
            <br />
            Delete Character:
            <br />
            <asp:DropDownList ID="DropDownListCharactersDeleteCharacter" runat="server" 
                DataSourceID="ObjectDataSourceCharacters" DataTextField="Name" 
                DataValueField="CharacterId">
            </asp:DropDownList>
            <asp:Button ID="ButtonDeleteCharacter" runat="server" Text="Delete" 
                onclick="ButtonDeleteCharacter_Click" />
            <asp:Label ID="DeleteCharacterMessageLabel" runat="server" ForeColor="Red"></asp:Label>
        </LoggedInTemplate>
    </asp:LoginView>
</asp:Content>
