using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Entities;
using System.Data.Common;
using System.Data;


namespace XiahBLL
{
    public abstract class ManagerBase
    {
        protected Database _db;

        public ManagerBase(string conString, string providerName)
        {
            _db = new Database(conString, providerName);
        }
    }
}
