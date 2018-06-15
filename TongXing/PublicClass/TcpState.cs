using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.Basics;
using System.Net.Sockets;

namespace SanNiuSignal.PublicClass
{
    /// <summary>
    /// TCP协议的state
    /// </summary>
    internal class TcpState : StateBase
    {
        private DateTime _heartTime = DateTime.Now;
        private bool _connectOk = false;//是否真正与对方相连接;主要用与服务器中的对象;
        /// <summary>
        /// 二个作用，客户端真正关闭了引擎；服务器:是否真正与对方相连接;主要用与服务器中的对象;
        /// </summary>
        internal bool ConnectOk
        {
            get { return _connectOk; }
            set { _connectOk = value; }
        }
        /// <summary>
        /// 处理粘包之用;如果有残留下面一个包和这个接上
        /// </summary>
        internal byte[] Residualpackage = null;
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="bufferSize">缓冲区</param>
        internal TcpState(Socket socket,int bufferSize)
            : base(socket, bufferSize)
        {
        }
        /// <summary>
        /// 心跳时间,接收到信息的时间，用于心跳设置
        /// </summary>
        internal DateTime HeartTime
        {
            get { return _heartTime; }
            set { _heartTime = DateTime.Now; }
        }
    }
}
