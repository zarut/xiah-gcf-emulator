using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class TopPets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GridViewTopPets_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                Label rowNumberLabel = e.Row.FindControl("RowNumberLabel") as Label;
                rowNumberLabel.Text = (e.Row.RowIndex+1).ToString();
            }
        }
    }
}