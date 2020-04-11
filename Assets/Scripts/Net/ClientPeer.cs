using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ClientPeer : MonoBehaviour
{
    private Socket clientSocket;
    private NetMsg msg;
    public ClientPeer()
    {
        try
        {
            msg = new NetMsg();
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }

    }
    public void Connect(string ip, int port)
    {
        try
        {
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            Debug.Log("connect to server successfully!");
            StartReceive();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }

    }
    #region receive data
    private byte[] receiveBuffer = new byte[1024];
    private List<byte> receiveCache = new List<byte>();
    private bool isProcessingReceive = false;
    public Queue<NetMsg> netMsgQueue = new Queue<NetMsg>();
    private void StartReceive()
    {
        if (clientSocket == null && clientSocket.Connected == false)
        {
            Debug.LogError("failed to connect");
            return;
        }
        clientSocket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, ReceiveCallback, clientSocket);
    }
    private void ReceiveCallback(IAsyncResult ar)
    {
        int length = clientSocket.EndReceive(ar);
        byte[] data = new byte[length];
        Buffer.BlockCopy(receiveBuffer, 0, data, 0, length);
        receiveCache.AddRange(data);
        if (isProcessingReceive == false)
        {
            ProcessReceive();
        }
        StartReceive();
    }
    private void ProcessReceive()
    {
        isProcessingReceive = true;
        byte[] packet = EncodeTool.DecodePacket(ref receiveCache);
        if (packet != null)
        {
            isProcessingReceive = false;
            return;
        }
        NetMsg msg = EncodeTool.DecodeMsg(packet);
        netMsgQueue.Enqueue(msg);
        ProcessReceive();
    }
    #endregion
    #region send data
    public void SendMsg(int opCode, int subCode, object value)
    {
        msg.Change(opCode, subCode, value);
        SendMsg(msg);
    }
    public void SendMsg(NetMsg msg)
    {
        try
        {
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);
            clientSocket.Send(packet);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

    }
    #endregion

}
