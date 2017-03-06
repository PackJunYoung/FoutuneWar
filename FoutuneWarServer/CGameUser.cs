using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;

namespace FoutuneWarServer
{
    public class CGameUser : IPeer
    {
        CUserToken token;

        public CGameRoom battleRoom { get; private set; }
        CPlayer player;

        public CGameUser(CUserToken token)
        {
            this.token = token;
            this.token.set_peer(this);
        }

        // 해당 유저로부터 메시지를 수신합니다.
        void IPeer.on_message(Const<byte[]> buffer)
        {
            byte[] clone = new byte[1024];
            Array.Copy(buffer.Value, clone, buffer.Value.Length);
            CPacket msg = new CPacket(clone, this);
            Program.gameMain.EnqueuePacket(msg, this);
        }

        // 해당 유저의 연결이 끊김.
        void IPeer.on_removed()
        {
            Console.WriteLine("The client disconnected.");
            Program.RemoveUser(this);
        }

        // 해당 유저에게 메시지를 송신합니다.
        public void send(CPacket msg)
        {
            token.send(msg);
        }

        // 해당 유저의 연결을 끊습니다.
        void IPeer.disconnect()
        {
            token.socket.Disconnect(false);
        }

        void IPeer.process_user_operation(CPacket msg)
        {
            PROTOCOL protocol = (PROTOCOL)msg.pop_protocol_id();
            Console.WriteLine("protocol id " + protocol);
            switch (protocol)
            {
                case PROTOCOL.LOG_IN:
                    {
                        CPacket p = CPacket.create((short)PROTOCOL.LOG_IN);
                        send(p);
                    }
                    break;
                case PROTOCOL.MATCHING_REQUEST:
                    {
                        Program.gameMain.MatchingRequest(this);
                    }
                    break;
                case PROTOCOL.LOADING_COMPLETED:
                    {
                        battleRoom.LoadingComplete(player);
                    }
                    break;
                case PROTOCOL.TURN_COMPLETED:
                    {
                        battleRoom.TurnComplete(player, msg);
                    }
                    break;
                case PROTOCOL.ROOM_EXIT:
                    {
                        battleRoom.Destroy();
                    }
                    break;
            }
        }

        // 게임방에 입장합니다.
        public void EnterRoom(CPlayer player, CGameRoom room)
        {
            this.player = player;
            battleRoom = room;
        }
    }
}
