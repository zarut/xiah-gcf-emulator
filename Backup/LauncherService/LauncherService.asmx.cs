using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using XiahBLL;
using System.Net.NetworkInformation;
using System.Configuration;

namespace XiahServices
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LauncherService : System.Web.Services.WebService
    {
        [WebMethod]
        public bool TryLogin(string username, string password, out string hashCode)
        {
            AccountManager accountManager = new AccountManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            bool loginSuccess = accountManager.TryLogin(username, password, out hashCode);

            return loginSuccess;
        }

        [WebMethod]
        public bool IsServerOn()
        {
            return true;
        }
    }
}
