using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XiahBLL;
using System.Configuration;
using System.Data.SqlClient;

namespace Web
{
    public partial class _Default : System.Web.UI.Page
    {
        CharacterManager characterManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            characterManager = new CharacterManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString,
                ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
        }

        protected void ButtonOk_Click(object sender, EventArgs e)
        {
            AccountManager accountManager = new AccountManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString,
                ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);

            TextBox UsernameTextBox = LoginView.FindControl("UsernameTextBox") as TextBox;
            TextBox PasswordTextBox = LoginView.FindControl("PasswordTextBox") as TextBox;
            Label UsernameExistsLabel = LoginView.FindControl("UsernameExistsLabel") as Label;

            if (Page.IsValid)
            {
                try
                {
                    accountManager.InsertUser(UsernameTextBox.Text, PasswordTextBox.Text);
                    Response.Redirect("~/AccountCreated.aspx");
                }
                catch (SqlException)
                {
                    UsernameExistsLabel.Text = "Username exists, choose a different one.";
                }
            }

        }

        protected void ChangeNicknameButton_Click(object sender, EventArgs e)
        {
            DropDownList DropDownListCharactersChangeNickname = LoginView.FindControl("DropDownListCharactersChangeNickname") as DropDownList;
            TextBox NewNicknameTextbox = LoginView.FindControl("NewNicknameTextbox") as TextBox;
            Label ChangeNicknameMessageLabel = LoginView.FindControl("ChangeNicknameMessageLabel") as Label;

            if (NewNicknameTextbox.MaxLength > 15)
            {
                ChangeNicknameMessageLabel.Text = "Nyb.";
            }

            if (Page.IsValid)
            {
                int returnValue =
                characterManager.ChangeCharacterNickname((int)Session["UserId"], int.Parse(DropDownListCharactersChangeNickname.SelectedValue), NewNicknameTextbox.Text);

                switch (returnValue)
                {
                    case -1:
                        ChangeNicknameMessageLabel.Text = "Nyb.";
                        break;
                    case 0:
                        ChangeNicknameMessageLabel.Text = "Nickname exists ofc.";
                        break;
                    case 1:
                        ChangeNicknameMessageLabel.Text = "Nickname changed, ty hf.";
                        Page.DataBind();
                        break;
                }

            }
        }

        protected void ResetSkillsButton_Click(object sender, EventArgs e)
        {
            DropDownList DropDownListCharactersResetSkills = LoginView.FindControl("DropDownListCharactersResetSkills") as DropDownList;
            Label ResetSkillsMessageLabel = LoginView.FindControl("ResetSkillsMessageLabel") as Label;

            int returnValue = characterManager.ResetCharacterSkills((int)Session["UserId"], int.Parse(DropDownListCharactersResetSkills.SelectedValue));

            switch (returnValue)
            {
                case -1:
                    ResetSkillsMessageLabel.Text = "Nyb";
                    break;
                case 0:
                    ResetSkillsMessageLabel.Text = "No skills, plz go learn some skills.";
                    break;
                case 1:
                    ResetSkillsMessageLabel.Text = "Skills reset, all done.";
                    Page.DataBind();
                    break;
            }
        }

        protected void ButtonDeleteCharacter_Click(object sender, EventArgs e)
        {
            DropDownList DropDownListCharactersDeleteCharacter = LoginView.FindControl("DropDownListCharactersDeleteCharacter") as DropDownList;
            Label DeleteCharacterMessageLabel = LoginView.FindControl("DeleteCharacterMessageLabel") as Label;

            int returnValue = characterManager.DeleteCharacter((int)Session["UserId"], int.Parse(DropDownListCharactersDeleteCharacter.SelectedValue));

            switch (returnValue)
            {
                case -1:
                    DeleteCharacterMessageLabel.Text = "Nyb.";
                    break;
                case 1:
                    DeleteCharacterMessageLabel.Text = "Character deleted, bb.";
                    Page.DataBind();
                    break;
            }
        }

    }
}
