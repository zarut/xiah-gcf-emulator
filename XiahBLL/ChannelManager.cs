using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using DAL;
using Entities;
using System.Data;


namespace XiahBLL
{
    public class ChannelManager : ManagerBase
    {
        public ChannelManager(string conString, string providerName)
            : base(conString, providerName)
        {

        }

        public ChannelInfo[] GetAllChannels()
        {
            _db.Open();

            List<ChannelInfo> channels = new List<ChannelInfo>();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETALLCHANNELS_STOREDPROC, System.Data.CommandType.StoredProcedure, null);

            int ordinalId = reader.GetOrdinal(DbNames.CHANNELINFO_ID);
            int ordinalName = reader.GetOrdinal(DbNames.CHANNELINFO_NAME);
            int ordinalDesc = reader.GetOrdinal(DbNames.CHANNELINFO_DESC);
            int ordinalIP = reader.GetOrdinal(DbNames.CHANNELINFO_IP);
            int ordinalPort = reader.GetOrdinal(DbNames.CHANNELINFO_PORT);
            int ordinalPK = reader.GetOrdinal(DbNames.CHANNELINFO_PK);
            while (reader.Read())
            {
                ChannelInfo channel = new ChannelInfo
                {
                    ChannelId = reader.GetInt32(ordinalId),
                    ChannelName = reader.GetString(ordinalName),
                    ChannelDesc = reader.GetString(ordinalDesc),
                    IPAddress = reader.GetString(ordinalIP),
                    Port = reader.GetInt32(ordinalPort),
                    PK = reader.GetBoolean(ordinalPK)
                };

                channels.Add(channel);
            }

            reader.Close();
            _db.Close();

            return channels.ToArray();
        }      
    }
}
