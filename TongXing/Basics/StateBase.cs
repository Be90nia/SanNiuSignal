using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace SanNiuSignal.Basics
{
    /// <summary>
    /// 一个State父类
    /// </summary>
    internal class StateBase
    {
       private Socket _workSocket = null;//工作的socket
       private IPEndPoint ipEndPoint = null;
       private int _bufferSize = 1024;//缓冲区大小
       private byte[] _buffer = null;//缓冲区
       private byte[] _sendDate = null;//已发送的数据
       private int _sendDateLabel = 0;//发送数据的标签
       private FileBase _sendFile = null;//有文件要发送了在这里进行设置
       private FileBase _receiveFile = null;//需要接收文件在这里进行设置
       private byte[] _bufferBackup = null;//备份缓冲区;主要是缓冲区有时候需要增大或缩小的时候用到；
        /// <summary>
       /// 备份缓冲区;主要是缓冲区有时候需要增大或缩小的时候用到；
        /// </summary>
       internal byte[] BufferBackup
       {
           get { return _bufferBackup; }
           set { _bufferBackup = value; }
       }
        /// <summary>
        /// 接收文件类
        /// </summary>
       internal FileBase ReceiveFile
       {
           get { return _receiveFile; }
           set { _receiveFile = value; }
       }
        /// <summary>
        /// 发送文件类
        /// </summary>
       internal FileBase SendFile
       {
           get { return _sendFile; }
           set { _sendFile = value; }
       }
        /// <summary>
        /// 缓冲区大小
        /// </summary>
       internal int BufferSize
       {
           get { return _bufferSize; }
       }
       /// <summary>
       /// 带参数的构造函数
       /// </summary>
       /// <param name="socket">Socket</param>
       /// <param name="bufferSize">缓冲区大小</param>
       internal StateBase(Socket socket, int bufferSize)
       {
           _bufferSize = bufferSize;
           _buffer = new byte[bufferSize];
           _workSocket = socket;
           try
           {
               ipEndPoint = (IPEndPoint)socket.RemoteEndPoint;
           }
           catch { }
       }
       /// <summary>
       /// 工作的Socket
       /// </summary>
       internal Socket WorkSocket
       {
           get { return _workSocket; }
           set { _workSocket = value; }
       }
       /// <summary>
       /// 缓冲区
       /// </summary>
       internal byte[] Buffer
       {
           get { return _buffer; }
           set { _buffer = value; }
       }
       /// <summary>
       /// 已发送的数据,主要用于对方没有收到信息可以重发用
       /// </summary>
       internal byte[] SendDate
       {
           get { return _sendDate; }
           set { _sendDate = value; }
       }
        /// <summary>
        /// 已发数据的标签
        /// </summary>
       internal int SendDateLabel
       {
           get { return _sendDateLabel; }
           set { _sendDateLabel = value; }
       }
        /// <summary>
        /// 同时设置发送数据和它的标签的方法
        /// </summary>
        /// <param name="Lable">标签</param>
        /// <param name="sendDate">已发送数据</param>
       internal void SendDateInitialization(int Lable,byte[] sendDate)
       {
           _sendDateLabel = Lable;
           _sendDate = sendDate;
       }
       /// <summary>
       /// IPEndPoint得到客户端地址,端口号；
       /// </summary>
       internal IPEndPoint IpEndPoint
       {
           get { return ipEndPoint; }
           set { ipEndPoint = value; }
       }
    }
}
