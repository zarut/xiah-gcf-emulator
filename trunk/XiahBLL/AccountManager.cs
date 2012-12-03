using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace XiahBLL
{
    public class AccountManager:ManagerBase
    {
        public AccountManager(string conString, string providerName)
            : base(conString, providerName)
        {

        }

        public bool TryLogin(string username, string password, out string hash)
        {
            bool loginSuccess = false;

            DateTime? loginTime = null;

            hash = "";

            DbParameter usernameParamater = _db.CreateParameter(DbNames.USERLOGIN_USERNAME_PARAMETER, username);
            DbParameter passwordParameter = _db.CreateParameter(DbNames.USERLOGIN_PASSWORD_PARAMETER, password);
            DbParameter connectionTimeParameter = _db.CreateParameter(DbNames.USERLOGIN_CONNECTIONTIME_PARAMETER, DbType.DateTime);
            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Int32);

            returnValueParameter.DbType = System.Data.DbType.Int32;
            returnValueParameter.Direction = System.Data.ParameterDirection.ReturnValue;

            usernameParamater.DbType = DbType.String;
            usernameParamater.Size = 20;
            passwordParameter.DbType = DbType.String;
            passwordParameter.Size = 20;

            connectionTimeParameter.Direction = System.Data.ParameterDirection.Output;

            _db.Open();



            _db.ExecuteNonQuery(DbNames.USERLOGIN_STOREDPROC, System.Data.CommandType.StoredProcedure, returnValueParameter, usernameParamater, passwordParameter, connectionTimeParameter);

            _db.Close();

            int returnValue = (int)returnValueParameter.Value;

            if (returnValue == 1)
            {
                loginTime = (DateTime)connectionTimeParameter.Value;
                hash = ((DateTime)loginTime).ToString(LogicNames.HASH_FORMATSTRING);
                loginSuccess = true;
            }


            return loginSuccess;
        }

        public bool LoginVerification(string username, ref int accountId)
        {
            bool loginSuccess = false;

            DbParameter usernameParamater = _db.CreateParameter(DbNames.LOGINVERIFICATION_USERNAME_PARAMETER, username);
            DbParameter accountIdParameter = _db.CreateParameter(DbNames.LOGINVERIFICATION_ACCOUNTID_PARAMETER, DbType.Int32);
            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Int32);

            returnValueParameter.DbType = System.Data.DbType.Int32;
            returnValueParameter.Direction = System.Data.ParameterDirection.ReturnValue;

            usernameParamater.DbType = System.Data.DbType.String;
            usernameParamater.Size = 20;

            accountIdParameter.Direction = System.Data.ParameterDirection.Output;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.LOGINVERIFICATION_STOREDPROC,
                System.Data.CommandType.StoredProcedure,
                returnValueParameter,
                usernameParamater,
                accountIdParameter);

            _db.Close();

            int returnValue = (int)returnValueParameter.Value;

            loginSuccess = (returnValue == 1);

            if (loginSuccess)
                accountId = (int)accountIdParameter.Value;

            return loginSuccess;
        }

        public bool ConnectVerification(int accountID)
        {
            bool connectSuccess = false;

            DbParameter accountIdParameter = _db.CreateParameter(DbNames.CONNECTVERIFICATION_ACCOUNTID_PARAMETER, accountID);
            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Int32);

            returnValueParameter.DbType = System.Data.DbType.Int32;
            returnValueParameter.Direction = System.Data.ParameterDirection.ReturnValue;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.CONNECTVERIFICATION_STOREDPROC,
                System.Data.CommandType.StoredProcedure,
                returnValueParameter,
                accountIdParameter);

            _db.Close();

            int returnValue = (int)returnValueParameter.Value;

            connectSuccess = (returnValue == 1);

            return connectSuccess;
        }

        public bool ExitGame(int accountID)
        {
            bool Success = false;

            DbParameter accountIdParameter = _db.CreateParameter(DbNames.CONNECTVERIFICATION_ACCOUNTID_PARAMETER, accountID);
            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Int32);

            returnValueParameter.DbType = System.Data.DbType.Int32;
            returnValueParameter.Direction = System.Data.ParameterDirection.ReturnValue;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.EXITGAME_STOREDPROC,
                System.Data.CommandType.StoredProcedure,
                returnValueParameter,
                accountIdParameter);

            _db.Close();

            int returnValue = (int)returnValueParameter.Value;

            Success = (returnValue == 1);

            return Success;
        }

        public void InsertUser(string username, string password)
        {
            DbParameter accountParameter = _db.CreateParameter(DbNames.INSERTUSER_ACCOUNT_PARAMETER, username);
            DbParameter passwordParameter = _db.CreateParameter(DbNames.INSERTUSER_PASSWORD_PARAMETER, password);
            accountParameter.DbType = DbType.String;
            passwordParameter.DbType = DbType.String;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.INSERTUSER_STOREDPROC, CommandType.StoredProcedure, accountParameter, passwordParameter);
            _db.Close();
        }


        public int GetUserIdByUsernameAndPassword(string username, string password)
        {
            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Int32);
            DbParameter usernameParamater = _db.CreateParameter(DbNames.USERLOGIN_USERNAME_PARAMETER, username);
            DbParameter passwordParameter = _db.CreateParameter(DbNames.USERLOGIN_PASSWORD_PARAMETER, password);
         
            returnValueParameter.DbType = System.Data.DbType.Int32;
            returnValueParameter.Direction = System.Data.ParameterDirection.Output;


            usernameParamater.DbType = DbType.String;
            usernameParamater.Size = 20;
            passwordParameter.DbType = DbType.String;
            passwordParameter.Size = 20;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.GETUSERIDBYUSERNAMEANDPASSWORD_STOREDPROC, CommandType.StoredProcedure, returnValueParameter, usernameParamater, passwordParameter);
            _db.Close();

            int userId = (int)returnValueParameter.Value;

            return userId;
        }

        public bool IsGM(int accountID)
        {
            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Boolean);
            DbParameter accountIDParameter = _db.CreateParameter(DbNames.ISGM_ACCOUNTID_PARAMETER, accountID);
            accountIDParameter.DbType = DbType.Int32;

            returnValueParameter.DbType = System.Data.DbType.Boolean;
            returnValueParameter.Direction = System.Data.ParameterDirection.Output;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.ISGM_STOREDPROC, CommandType.StoredProcedure, returnValueParameter, accountIDParameter);
            _db.Close();

            bool isGM = (bool)returnValueParameter.Value;

            return isGM;
        }
    }
}
