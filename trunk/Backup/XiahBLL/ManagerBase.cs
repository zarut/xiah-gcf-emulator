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
        protected Random rand;

        public ManagerBase(string conString, string providerName)
        {
            _db = new Database(conString, providerName);
            rand = new Random();
        }

        public bool PercentSuccess(double percent)
        {
            return ((double)rand.Next(1, 1000000)) / 10000 >= 100 - percent;
        }
    }
}
