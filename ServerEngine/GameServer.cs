using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerEngine
{
    class GameServer : ServerBase
    {
        public GameServer(string ipAddress, int port)
            :base(ipAddress, port)
        {

        }
    }
}
