using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;

namespace FoutuneWarServer
{
    public class CGameRoom
    {
        // 게임을 진행하는 플레이어
        List<CPlayer> players;

        // 플레이어들의 상태를 관리하는 변수.
        Dictionary<byte, PLAYER_STATE> playerState;

        int[] redTeamIdx = new int[3];
        int[] blueTeamIdx = new int[3];

        public CGameRoom()
        {
            players = new List<CPlayer>();
            playerState = new Dictionary<byte, PLAYER_STATE>();
        }

        // 모든 유저들에게 메시지를 송신합니다.
        void Broadcast(CPacket msg)
        {
            players.ForEach(player => player.SendForBroadcast(msg));
            CPacket.destroy(msg);
        }

        // 플레이어의 상태를 변경합니다.
        void ChangePlayerState(CPlayer player, PLAYER_STATE state)
        {
            if (playerState.ContainsKey(player.playerIndex))
                playerState[player.playerIndex] = state;
            else
                playerState.Add(player.playerIndex, state);
        }

        // 모든 플레이어가 임의의 상태가 되었는지 검사합니다.
        bool CheckAllPlayerState(PLAYER_STATE state)
        {
            foreach (KeyValuePair<byte, PLAYER_STATE> kvp in playerState)
            {
                if (kvp.Value != state)
                    return false;
            }
            return true;
        }

        // 매칭이 완료된 플레이어들을 방에 입장시킵니다.
        public void EnterGameRoom(CGameUser user1, CGameUser user2)
        {
            // 플레이어들을 생성하고 각각 인덱스를 할당합니다.
            CPlayer player1 = new CPlayer(user1, 0);
            CPlayer player2 = new CPlayer(user2, 1);
            players.Clear();
            players.Add(player1);
            players.Add(player2);

            // 플레이어들의 초기 상태를 지정해 준다.
            playerState.Clear();
            ChangePlayerState(player1, PLAYER_STATE.ENTERED_ROOM);
            ChangePlayerState(player2, PLAYER_STATE.ENTERED_ROOM);

            // 로딩 시작메시지 전송.
            players.ForEach(player =>
            {
                CPacket msg = CPacket.create((short)PROTOCOL.START_LOADING);
                msg.push(player.playerIndex);
                player.Send(msg);
            });

            user1.EnterRoom(player1, this);
            user2.EnterRoom(player2, this);
        }

        // 플레이어가 로딩을 완료한 후에 호출됩니다.
        public void LoadingComplete(CPlayer player)
        {
            // 해당 플레이어를 로딩완료 상태로 변경합니다.
            ChangePlayerState(player, PLAYER_STATE.LOADING_COMPLETE);

            // 모든 유저가 로딩완료 상태인지 검사합니다.
            if (!CheckAllPlayerState(PLAYER_STATE.LOADING_COMPLETE))
                return;

            // 모두 준비가 되었다면 게임시작.
            StartBattle();
        }

        // 플레이어가 턴을 완료한 후에 호출됩니다.
        public void TurnComplete(CPlayer player, CPacket msg)
        {
            ChangePlayerState(player, PLAYER_STATE.TURN_COMPLETE);

            if (player.playerIndex == 0)
            {
                redTeamIdx[0] = msg.pop_int32();
                redTeamIdx[1] = msg.pop_int32();
                redTeamIdx[2] = msg.pop_int32();                
            }
            else
            {
                blueTeamIdx[0] = msg.pop_int32();
                blueTeamIdx[1] = msg.pop_int32();
                blueTeamIdx[2] = msg.pop_int32();
            }

            // 모든 유저가 턴완료 상태인지 검사합니다.
            if (!CheckAllPlayerState(PLAYER_STATE.TURN_COMPLETE))
                return;

            SendTurnComplete();
        }

        void SendTurnComplete()
        {
            CPacket msg = CPacket.create((short)PROTOCOL.TURN_COMPLETED);
            msg.push(redTeamIdx[0]);
            msg.push(redTeamIdx[1]);
            msg.push(redTeamIdx[2]);
            msg.push(blueTeamIdx[0]);
            msg.push(blueTeamIdx[1]);
            msg.push(blueTeamIdx[2]);

            Broadcast(msg);
        }

        // 게임을 시작합니다.
        void StartBattle()
        {
            ResetGameData();

            CPacket msg = CPacket.create((short)PROTOCOL.GAME_START);
            Broadcast(msg);
        }

        void ResetGameData()
        {

        }

        // 게임방을 제거합니다.
        public void Destroy()
        {
            CPacket msg = CPacket.create((short)PROTOCOL.ROOM_REMOVED);
            Broadcast(msg);

            players.Clear();
        }
    }

    public enum PLAYER_STATE : byte
    {
        // 입장한 상태.
        ENTERED_ROOM,

        // 로딩을 완료한 상태.
        LOADING_COMPLETE,

        // 턴을 완료한 상태.
        TURN_COMPLETE,

        // 입력 진행 상태.
        PLAY,
    }
}
