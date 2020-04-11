using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMsgCenter : MonoBehaviour
{
    public static NetMsgCenter Instance;
    private ClientPeer client;
    private void Awake()
    {
        Instance = this;
        client = new ClientPeer();
        client.Connect("127.0.0.1", 6666);
    }
    private void FixedUpdate()
    {
        if (client == null)
        {
            return;
        }
        while (client.netMsgQueue.Count > 0)
        {
            NetMsg msg = client.netMsgQueue.Dequeue();
            ProcessServerSendMsg(msg);
        }
    }
    #region send message
    public void SendMsg(int opCode,int subCode,object value)
    {
        client.SendMsg(opCode, subCode, value);
    }
    public void SendMsg(NetMsg msg)
    {
        client.SendMsg(msg);
    }
    #endregion
    #region process received data from server
    AccountHandler accountHandler = new AccountHandler();
    MatchHandler matchHandler = new MatchHandler();
    ChatHandler chatHandler = new ChatHandler();
    FightHandler fightHandler = new FightHandler();
    private void ProcessServerSendMsg(NetMsg msg)
    {
        switch (msg.opCode)
        {
            case OpCode.Account:
                accountHandler.OnReceive(msg.subCode, msg.value);
                break;
            case OpCode.Chat:
                chatHandler.OnReceive(msg.subCode, msg.value);
                break;
            case OpCode.Match:
                matchHandler.OnReceive(msg.subCode, msg.value);
                break;
            case OpCode.Fight:
                fightHandler.OnReceive(msg.subCode, msg.value);
                break;
            default:
                break;
        }
    }
    #endregion

}
