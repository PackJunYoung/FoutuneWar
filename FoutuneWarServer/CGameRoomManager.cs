using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoutuneWarServer
{
    class CGameRoomManager
    {
        List<CGameRoom> rooms;

        public CGameRoomManager()
        {
            rooms = new List<CGameRoom>();
        }

        // 게임방을 생성합니다.
        public void CreateRoom(CGameUser user1, CGameUser user2)
        {
            // 게임 방을 생성하여 입장 시킴.
            CGameRoom battleRoom = new CGameRoom();
            battleRoom.EnterGameRoom(user1, user2);

            rooms.Add(battleRoom);
        }

        // 게임방을 제거합니다.
        public void RemoveRoom(CGameRoom room)
        {
            room.Destroy();
            rooms.Remove(room);
        }
    }
}
