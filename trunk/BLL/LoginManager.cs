using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data.Common;
using System.Data.SqlClient;

namespace BLL
{
    public class LoginManager
    {
        private WebDatabase _db;

        public LoginManager(string conStringName)
        {
            _db = new WebDatabase(conStringName);
        }

        public bool TryLogin(string username, string password, out string hash)
        {
            bool loginSuccess = false;
            _db.Open();

            hash = "";
            DbParameter[] parameters = new DbParameter[] { new SqlParameter("@username", username), 
                    new SqlParameter("@password", password),
                new SqlParameter("@connectionTime", System.Data.SqlDbType.DateTime) };
            parameters[2].Direction = System.Data.ParameterDirection.Output;

            int userCount = (int)_db.ExecuteScalar(DbNames.USERLOGIN_STOREDPROC,
                System.Data.CommandType.StoredProcedure, parameters);

            if (userCount == 1)
            {
                DateTime loginTime = (DateTime)parameters[2].Value;
                hash = loginTime.ToString("yyyyMMddHHmmss");
                loginSuccess = true;
            }

            _db.Close();

            return loginSuccess;
        }
    }
}
