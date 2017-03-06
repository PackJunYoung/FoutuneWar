using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;
using FoutuneWarServer.Entities;
using FoutuneWarServer.NHibernate;

namespace FoutuneWarServer
{
    class Program
    {
        static List<CGameUser> userList;
        public static CGameServer gameMain = new CGameServer();

        static void Main(string[] args)
        {
            CPacketBufferManager.initialize(2000);
            userList = new List<CGameUser>();

            CNetworkService service = new CNetworkService();
            service.session_created_callback += OnSessionCreated;
            service.initialize();
            service.listen("0.0.0.0", 7979, 100);

            Console.WriteLine("Started!");
            while (true)
            {
                string input = Console.ReadLine();
                System.Threading.Thread.Sleep(1000);
            }

            Console.ReadKey();
        }

        // 새로운 세션(유저)가 추가되었을 때 호출.
        static void OnSessionCreated(CUserToken token)
        {
            CGameUser user = new CGameUser(token);
            lock (userList)
            {
                userList.Add(user);
            }
        }

        // 유저의 접속이 끊겼을 때 호출.
        public static void RemoveUser(CGameUser user)
        {
            lock (userList)
            {
                userList.Remove(user);
                gameMain.UserDisconnected(user);

                CGameRoom room = user.battleRoom;
                if (room != null)
                {
                    gameMain.roomManager.RemoveRoom(user.battleRoom);
                }
            }
        }
    }
}
