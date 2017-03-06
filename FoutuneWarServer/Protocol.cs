using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoutuneWarServer
{
    public enum PROTOCOL : short
    {
        BEGIN = 0,

        // 로그인.
        LOG_IN = 1,

        // 매칭 요청.
        MATCHING_REQUEST = 2,

        // 로딩 시작.
        START_LOADING = 3,

        // 게임 종료.
        ROOM_REMOVED = 4,

        // 로딩 완료.
        LOADING_COMPLETED = 5,

        // 게임 시작.
        GAME_START = 6,

        // 매칭 취소.
        MATCHING_CANCEL = 7,

        // 턴 완료.
        TURN_COMPLETED = 8,

        // 방 나가기.
        ROOM_EXIT = 9,

        END
    }
}
