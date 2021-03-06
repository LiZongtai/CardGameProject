﻿public enum EventDefine
{
    ShowRegisterPanel,
    ShowLoginPanel,
    Hint,
    ShowRankListPanel,
    SendRankListDto,
    ShowRechargePanel,
    UpdateCoinCount,
    ShowRoomChoosePanel,
    //VSAI,
    //VSWithSelf,
    //GameOver,

    //联网部分
    //RefreshUI,
    //StartGame,
    //LeftDealCard,
    //RightDealCard,
    //SelfDealCard,
    //LeftBanker,
    //RightBanker,
    //SelfBanker,
    //LeaveFightRoom,
    //StartStakes,
    //LookCardBRO,
    //PutStakesBRO,
    //GiveUpCardBRO,
    //CompareCardBRO,
    //GameOverBRO,
    //ChatBRO,

    //Game Manager
    Player2,
    Player3,
    Player4,
    EnemySci,
    SciSelected,
    CardShowing,
    CardEndShowing,
    CardActing,
    StopCardActing,
    PatentShowing,
    PatentEndShowing,
    InitHandCard,

    //Acting
    CardDiscard,
    CardDraw,
    CardDiscardFinish,
    DrawCard0,
    DrawCard1,
    DrawCard2,
    DrawCard3,
    DrawCardFinish,
    Card2Patent,

    IEaction,
    Infiltration,
    InfiltrationRes,
    InfiltrationFinish,
    Defense,
    DefenseFinish,
    Acted,
    MaintenanceFinish,
    DiscardFinish,
    CardShow,
    CardShowFinish,

    //Game Round
    GameStart,
    s0_RoundStart,
    s1_PlayRoundStart,
    s2_BeforeRound,
    s3_BeforeAction,
    s4_AfterAction,
    s5_BeforeDraw,
    s6_BeforeDiscard,
    s7_AfterDiscard,
    s8_BeforeMaintenance,
    s9_PlayerRoundEnd,
    s10_RoundEnd,
    r0_attack,
    r1_attacked,
    r2_defend,
    r3_defended,

    //Card
    CardSE,

}
