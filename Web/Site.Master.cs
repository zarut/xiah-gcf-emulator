using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XiahBLL;
using System.Configuration;
using System.Web.Security;

namespace Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void ButtonLoginOk_Click(object sender, EventArgs e)
        {
            AccountManager accountManager = new AccountManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString,
               ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);

            TextBox UsernameTextBox = LoginView.FindControl("UsernameTextBox") as TextBox;
            TextBox PasswordTextBox = LoginView.FindControl("PasswordTextBox") as TextBox;
            Label WrongUsernamePasswordLabel = LoginView.FindControl("WrongUsernamePasswordLabel") as Label;

            if (Page.IsValid)
            {
                int userId = accountManager.GetUserIdByUsernameAndPassword(UsernameTextBox.Text, PasswordTextBox.Text);

                if (userId < 1)
                {
                    WrongUsernamePasswordLabel.Text = "Wrong username or password.";
                }
                else
                {
                    Session["UserId"] = userId;
                    FormsAuthentication.RedirectFromLoginPage(UsernameTextBox.Text, false);
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}
