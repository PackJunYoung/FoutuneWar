using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;
using System.Threading;

namespace FoutuneWarServer
{
    class CGameServer
    {
        object operationLock;

        Queue<CPacket> userOperations;

        Thread logicThread;
        AutoResetEvent loopEvent;

        public CGameRoomManager roomManager { get; private set; }
        List<CGameUser> matchingWaitingUsers;

        public CGameServer()
        {
            operationLock = new object();
            loopEvent = new AutoResetEvent(false);
            userOperations = new Queue<CPacket>();

            roomManager = new CGameRoomManager();
            matchingWaitingUsers = new List<CGameUser>();

            logicThread = new Thread(GameLoop);
            logicThread.Start();
        }

        // 게임로직을 수행하는 루프입니다.
        void GameLoop()
        {
            while (true)
            {
                CPacket packet = null;
                lock (operationLock)
                {
                    if (userOperations.Count > 0)
                    {
                        packet = userOperations.Dequeue();
                    }
                }

                if (packet != null)
                {
                    // 패킷 처리.
                    ProcessReceive(packet);
                }

                // 더이상 처리할 패킷이 없을 경우 스레드 대기.
                if (userOperations.Count <= 0)
                {
                    loopEvent.WaitOne();
                }
            }
        }

        // 패킷을 추가합니다.
        public void EnqueuePacket(CPacket packet, CGameUser user)
        {
            lock (operationLock)
            {
                userOperations.Enqueue(packet);
                loopEvent.Set();
            }
        }

        void ProcessReceive(CPacket msg)
        {
            msg.owner.process_user_operation(msg);
        }

        // 유저로부터 매칭 요청이 왔을 때 호출됩니다.
        public void MatchingRequest(CGameUser user)
        {
            if (matchingWaitingUsers.Contains(user))
                return;

            // 매칭 대기 리스트에 추가.
            matchingWaitingUsers.Add(user);

            // 2명이 모이면 매칭 성공.
            if (matchingWaitingUsers.Count == 2)
            {
                // 게임방 생성.
                roomManager.CreateRoom(matchingWaitingUsers[0], matchingWaitingUsers[1]);

                // 매칭 대기 리스트 삭제.
                matchingWaitingUsers.Clear();
            }
        }

        public void UserDisconnected(CGameUser user)
        {
            if (matchingWaitingUsers.Contains(user))
                matchingWaitingUsers.Remove(user);
        }
    }
}
