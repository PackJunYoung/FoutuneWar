using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;

namespace FoutuneWarServer
{
    public class CPlayer
    {
        CGameUser owner;
        public byte playerIndex { get; private set; }
        
        public CPlayer(CGameUser user, byte playerIndex)
        {
            owner = user;
            this.playerIndex = playerIndex;
        }

        public void Send(CPacket msg)
        {
            owner.send(msg);
            CPacket.destroy(msg);
        }

        public void SendForBroadcast(CPacket msg)
        {
            owner.send(msg);
        }
    }
}
